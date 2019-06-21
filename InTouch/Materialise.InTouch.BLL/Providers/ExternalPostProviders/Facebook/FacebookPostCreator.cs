using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models;
using Materialise.InTouch.BLL.Services.Storage.Models;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook
{
    public class FacebookPostCreator : IFacebookPostCreator
    {
        private readonly IFacebookClient _facebookClient;
        private readonly IFileService _fileService;

        public FacebookPostCreator(IFacebookClient facebookClient, IFileService fileService)
        {
            _facebookClient = facebookClient;
            _fileService = fileService;
        }

        public async Task<string> GetPageName(string pageId)
        {
            if (String.IsNullOrEmpty(pageId))
            {
                throw new ArgumentNullException(nameof(pageId), "PageId is not defined.");
            }
            var name = await _facebookClient.GetPageName(pageId);
            return name;
        }

        public async Task<IList<AttachmentData>> GetPostAttachments(string postId)
        {
            if (String.IsNullOrEmpty(postId))
            {
                throw new ArgumentNullException(nameof(postId), "PostId is not defined.");
            }
            var attachments = await _facebookClient.GetPostAttachments(postId);
            return attachments;
        }

        public async Task<IList<FacebookPost>> GetRawPosts(string pageId)
        {
            if (String.IsNullOrEmpty(pageId))
            {
                throw new ArgumentNullException(nameof(pageId), "PageId is not defined.");
            }
            var posts = await _facebookClient.GetPosts(pageId);
            return posts;
        }

        public async Task<ICollection<PostFile>> GetPostImages(AttachmentData attachments, string postId)
        {
            var fileList = new List<PostFile>();

            var arrayOfImages = attachments.Subattachments?.Data ?? new List<SubAttachmentData>();
            if (arrayOfImages.Count == 0)
            {
                var mainImage = attachments.Media?.Image;
                if (mainImage != null)
                {
                    arrayOfImages.Add(new SubAttachmentData
                    {
                        Media = new Media
                        {
                            Image = mainImage
                        }
                    });
                }
            }
            foreach (var image in arrayOfImages)
            {
                var info = await GetFileInfo(image, postId);
                if (info != null)
                {
                    fileList.Add(FileMapper.ConvertToPostFile(info));
                }
            }
            return fileList;
        }

        public async Task<FileInfoDTO> GetFileInfo(SubAttachmentData image, string postId)
        {
            using (var imageResponse = await _facebookClient.GetImage(image.Media.Image.Src))
            {
                using (var imageStream = await imageResponse.Content.ReadAsStreamAsync())
                {
                    var mediaType = imageResponse.Content.Headers.ContentType.MediaType.Split('/');
                    if (mediaType.FirstOrDefault() == "image")
                    {

                        var fileRequest = new CreateFileRequest
                        {
                            Name = "FacebookImg_" + postId + "." + mediaType.LastOrDefault(),
                            ContentType = imageResponse.Content.Headers.ContentType.MediaType,
                            SizeKB = Convert.ToInt32(imageResponse.Content.Headers.ContentLength)
                        };

                        return await _fileService.CreateAsync(fileRequest, imageStream);
                    }
                    return null;
                }
            }
        }

    }
}
