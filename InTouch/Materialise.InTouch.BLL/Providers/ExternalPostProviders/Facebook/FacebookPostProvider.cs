using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models.Config;
using Materialise.InTouch.DAL.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook
{
    public class FacebookPostProvider : IExternalPostProvider
    {
        private readonly string _titleBuilder;
        private readonly List<FacebookPage> _pagesId;
        private readonly IFacebookPostCreator _facebookPostCreator;

        public FacebookPostProvider(IOptionsSnapshot<FacebookPostProviderConfig> options, IFacebookPostCreator facebookPostCreator)
        {
            _facebookPostCreator = facebookPostCreator;
            _titleBuilder = options.Value.DefaultTitle;
            _pagesId = options.Value.Pages;
        }

        public async Task<List<Post>> GetPostsAsync(DateTime? lastSyncDate)
        {
            var importedPosts = new List<Post>();
            foreach (var page in _pagesId)
            {
                var facebookPosts = await _facebookPostCreator.GetRawPosts(page.PageId);
                var defaultTitle = _titleBuilder + await _facebookPostCreator.GetPageName(page.PageId);

                foreach (var fbPost in facebookPosts)
                {
                    var isNew = DateTime.Parse(fbPost.Created_time) > lastSyncDate;
                    var hasContent = !string.IsNullOrEmpty(fbPost.Message);

                    if (isNew && hasContent)
                    {
                        var parsedPost = ExternalPostMapper.ConvertFacebookPostToPost(fbPost, defaultTitle);

                        var attachments = await _facebookPostCreator.GetPostAttachments(fbPost.Id);
                        if (attachments.FirstOrDefault() != null)
                        {
                            var attachmentTitle = attachments.First().Title;
                            if (!string.IsNullOrEmpty(attachmentTitle) && attachmentTitle.Length < 100)
                            {
                                parsedPost.Title = attachmentTitle;
                            }

                            if (attachments.First().Type == "share")
                            {
                                parsedPost.Content +=
                                    "Link: <a href=\"" + attachments.First().Url + "\" target=\"_blank\">" + attachments.First().Title+"</a>";
                            }
                            parsedPost.PostFiles = await _facebookPostCreator.GetPostImages(attachments.First(), fbPost.Id);
                        }
                        importedPosts.Add(parsedPost);
                    }
                }
            }
            return importedPosts;
        }
    }
}