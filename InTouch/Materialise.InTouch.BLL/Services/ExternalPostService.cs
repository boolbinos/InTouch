using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.DAL.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Materialise.InTouch.BLL.Services.PostConfiguration;

namespace Materialise.InTouch.BLL.Services
{
    public class ExternalPostService : IExternalPostService
    {
        private readonly IExternalPostProviderFactory _postProviderFactory;
        private readonly IUnitOfWork _db;
        private readonly ILogger<ExternalPostService> _log;
        private readonly IOptionsSnapshot<PostConfig> _postDefaultValuesOptions;

        private static SemaphoreSlim _semaphore;

        static ExternalPostService()
        {
            _semaphore = new SemaphoreSlim(1, 1);
        }
        public ExternalPostService(IExternalPostProviderFactory postProviderFactory, IUnitOfWork db, ILogger<ExternalPostService> log, IOptionsSnapshot<PostConfig> postDefaultValuesOptions)
        {
            _postProviderFactory = postProviderFactory;
            _db = db;
            _log = log;
            _postDefaultValuesOptions = postDefaultValuesOptions;
        }

        public async Task<int> ImportPostsAsync(string providerName)
        {
            if (string.IsNullOrEmpty(providerName))
            {
                throw new ArgumentNullException("Invalid parameter", nameof(providerName));
            }
            var stringContainsInvalidCharacters = providerName.Any(c => int.TryParse(c.ToString(), out var _));
            if (stringContainsInvalidCharacters)
            {
                throw new ArgumentException("Invalid characters", nameof(providerName));
            }
            PostType providerType;

            try
            {
                providerType = (PostType)Enum.Parse(typeof(PostType), providerName, true);
            }
            catch
            {
                throw new ArgumentException("Unknown provider", nameof(providerName));
            }

            var postsImported = 0;

            await _semaphore.WaitAsync();

            try
            {

                var lastSyncDate = default(DateTime);
                var existingPosts = await _db.Posts.FindAsync(post => post.PostType == providerType);
                if (existingPosts.Count != 0)
                {
                    var lastPost = existingPosts.OrderBy(post => post.CreatedDate).Last();
                    lastSyncDate = lastPost.CreatedDate;
                }

                var provider = GetProviderByType(providerType);

                var posts = await provider.GetPostsAsync(lastSyncDate);

                foreach (var post in posts)
                {
                    var defaultEndDateTime = _postDefaultValuesOptions.Value.DisplayPeriodInDaysForAdmins;

                    post.StartDateTime = DateTime.Now;
                    post.EndDateTime = post.StartDateTime.AddDays(defaultEndDateTime);
                    await _db.Posts.CreateAsync(post);
                    postsImported++;
                }

                await _db.SaveAsync();
            }
            finally
            {
                _semaphore.Release();
            }
            _log.LogInformation($"Import from {providerName} successfully completed: {postsImported} have been imported");
            return postsImported;
        }

        private IExternalPostProvider GetProviderByType(PostType providerType)
        {
            switch (providerType)
            {
                case PostType.Facebook:
                    {
                        return _postProviderFactory.GetFacebookPostProvider();
                    }
                case PostType.Sharepoint:
                    {
                        return _postProviderFactory.GetSharepointPostProvider();
                    }
                default:
                    {
                        throw new NotImplementedException($"Provider \"{providerType}\" is not implemented yet");
                    }
            }
        }
    }
}