using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Configs;
using Materialise.InTouch.BLL.Enumes;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.ModelsDTO;
using Microsoft.Extensions.Options;

namespace Materialise.InTouch.BLL.Services
{
    public class FullscreenBatchService : IFullscreenBatchService
    {
        private readonly IOptionsSnapshot<FullscreenBatchConfig> _batchOptions;

        public FullscreenBatchService(IOptionsSnapshot<FullscreenBatchConfig> batchOptions)
        {
            this._batchOptions = batchOptions;
        }

        public Task<List<FullscreenPostPartDTO>> CreateBatchAsync(List<PostDTO> posts)
        {
            return Task.Run(() =>
            {
                var batch = posts.SelectMany(SplitPost).ToList();
                return batch;
            });
        }

        public List<FullscreenPostPartDTO> SplitPost(PostDTO post)
        {
            var splittedPostList = new List<FullscreenPostPartDTO>();

            var postContainsImages = post.Files.Any();
            if (postContainsImages)
            {
                splittedPostList.AddRange(GetPostPartsWithImages(post));
            }
            else
            {
                splittedPostList.Add(GetPostWithTitle(post));
            }

            var postContainsVideo = post.VideoUrl != null;
            if (postContainsVideo)
            {
                splittedPostList.Add(GetPostPartWithVideo(post));
            }

            return splittedPostList;
        }

        public FullscreenPostPartDTO GetPostWithTitle(PostDTO post)
        {
            var postWithTitle = PostMapper.ConvertToFullscreenPostPartWithNoContent(post);

            postWithTitle.Title = post.Title;
            postWithTitle.ContentType = ContentType.Title;
            postWithTitle.DurationInSeconds = _batchOptions.Value.TitlePostPartDurationInSeconds;

            return postWithTitle;
        }

        public FullscreenPostPartDTO GetPostPartWithVideo(PostDTO post)
        {
            var postWithVideo = PostMapper.ConvertToFullscreenPostPartWithNoContent(post);

            postWithVideo.VideoUrl = post.VideoUrl;
            postWithVideo.ContentType = ContentType.Video;
            postWithVideo.DurationInSeconds = post.DurationInSeconds;

            return postWithVideo;
        }

        public IEnumerable<FullscreenPostPartDTO> GetPostPartsWithImages(PostDTO post)
        {
            var postContainsImages = post.Files.Any();
            if (postContainsImages)
            {
                var imagePartsPerPost = _batchOptions.Value.FullscreenMaxImagePerPostNumber;
                if(imagePartsPerPost == 0)
                {
                    imagePartsPerPost = 1;
                } 

                var selectedPortionOfFiles = post.Files.OrderBy(x => new Random().Next()).ToList();

                selectedPortionOfFiles.Remove(post.Files.First());
                selectedPortionOfFiles = selectedPortionOfFiles.Prepend(post.Files.First()).ToList();

                selectedPortionOfFiles = selectedPortionOfFiles.Take(imagePartsPerPost).ToList();

                foreach (var file in selectedPortionOfFiles)
                {
                    var postWithImage = PostMapper.ConvertToFullscreenPostPartWithNoContent(post);
                    postWithImage.File = file;

                    var isFirstImage = file == post.Files.First();
                    if (isFirstImage)
                    {
                        postWithImage.Title = post.Title;
                        postWithImage.ContentType = ContentType.TitleImage;
                        postWithImage.DurationInSeconds = _batchOptions.Value.TitlePostPartDurationInSeconds;
                    }
                    else
                    {
                        postWithImage.ContentType = ContentType.Image;
                        postWithImage.DurationInSeconds = _batchOptions.Value.ImagePostPartDurationInSeconds;
                    }

                    yield return postWithImage;
                }
            }
        }
    }
}