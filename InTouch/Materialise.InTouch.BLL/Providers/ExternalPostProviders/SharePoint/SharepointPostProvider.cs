using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models.Config;
using Materialise.InTouch.DAL.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint
{
    public class SharepointPostProvider : IExternalPostProvider
    {
        private List<SharePointList> _lists;
        private ISharePointPostCreator _sharePointPostCreator;

        public SharepointPostProvider(ISharePointPostCreator sharePointPostCreator, IOptionsSnapshot<SharePointPostProviderConfig> options)
        {
            _lists = options.Value.Lists;
            _sharePointPostCreator = sharePointPostCreator;
        }

        public async Task<List<Post>> GetPostsAsync(DateTime? lastSyncDate)
        {
            var importedPost = new List<Post>();

            foreach (var list in _lists)
            {
                var posts = await GetPostsWithImages(lastSyncDate, list);
                importedPost.AddRange(posts);
            }

            return importedPost;
        }

        private async Task<IList<Post>> GetPostsWithImages(DateTime? lastSyncDate, SharePointList list)
        {
            var posts = new List<Post>();
            var sharePointPosts = await _sharePointPostCreator.GetPosts(list);


            foreach (var spPost in sharePointPosts)
            {
                var isNew = spPost.Created > lastSyncDate;
                var hasContent = !string.IsNullOrEmpty(spPost.Body) && !string.IsNullOrEmpty(spPost.Title);

                if (isNew && hasContent)
                {
                    var parsedPost = ExternalPostMapper.ConvertSharePointPostToPost(spPost);
                    parsedPost.PostFiles = new List<PostFile>();
                    if (spPost.ImagesInBodyPost.Count!=0 )
                    {
                        var getImages = await _sharePointPostCreator.GetPostImages(spPost.ImagesInBodyPost, spPost.Id);
                        AddImages(getImages, parsedPost.PostFiles);
                    }

                    if (spPost.Attachment)
                    {
                        var getImages = await _sharePointPostCreator.GetImages(list, spPost.Id);
                        var getConvertImages = await _sharePointPostCreator.GetPostImages(getImages, spPost.Id);
                        AddImages(getConvertImages,parsedPost.PostFiles);
                    }
                    posts.Add(parsedPost);
                }
            }

            return posts;
        }
        private void AddImages(ICollection<PostFile> src, ICollection<PostFile> dest)
        {
            foreach (var image in src)
            {
                dest.Add(image);
            }
        }
        
    }
}
