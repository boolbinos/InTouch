using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models.Config;
using Materialise.InTouch.BLL.Services.Storage.Models;
using Materialise.InTouch.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint
{
    public class SharePointPostCreator : ISharePointPostCreator
    {
        private readonly ISharePointClient _sharePointClient;
        private readonly IFileService _fileService;

        public SharePointPostCreator(ISharePointClient sharePointClient, IFileService fileService)
        {
            _sharePointClient = sharePointClient;
            _fileService = fileService;
        }

        public async Task<IList<SharepointPost>> GetPosts(SharePointList list)
        {
            var posts = await _sharePointClient.GetListPostAsync(list.Title, list.BasePathExtension);

            return posts;
        }

        public async Task<IList<SharepointPostImages>> GetImages(SharePointList list, int id)
        {
            var files = await _sharePointClient.GetImages(list.Title, id, list.BasePathExtension);

            return files;
        }
        public async Task<ICollection<PostFile>> GetPostImages(IList<SharepointPostImages> attachments, int postId)
        {
            var fileList = new List<PostFile>();
            foreach (var image in attachments)
            {
                var info = await GetFileInfo(image, postId);
                fileList.Add(FileMapper.ConvertToPostFile(info));
            }
            return fileList;
        }
        private async Task<FileInfoDTO> GetFileInfo(SharepointPostImages image, int postId)
        {
            var fileRequest = new CreateFileRequest
            {
                Name = postId.ToString() + image.Name,
                ContentType = image.ContentType,
                SizeKB = Convert.ToInt32(image.file.Length)
            };
            return await _fileService.CreateAsync(fileRequest, image.file);
        }
    }
}
