using System;
using System.IO;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.BLL.Services.Storage.Models;
using Materialise.InTouch.DAL.Interfaces;
using Microsoft.Extensions.Options;
using FileInfo = Materialise.InTouch.DAL.Entities.FileInfo;
using System.Collections.Generic;
using System.Linq;
using Materialise.InTouch.BLL.Enumes;

namespace Materialise.InTouch.BLL.Services.Storage
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _db;
        private readonly IOptionsSnapshot<StorageConfig> _options;
        private readonly IUserContext _userContext;

        public FileService()
        {
        }

        public FileService(IUnitOfWork db, IOptionsSnapshot<StorageConfig> options, IUserContext userContext)
        {
            _db = db;
            _options = options;
            _userContext = userContext;
        }

        private string StorageFolderPath => _options.Value.FolderPath;

        public async Task<FileInfoDTO> CreateAsync(CreateFileRequest file, Stream data)
        {
            if (!Directory.Exists(StorageFolderPath))
            {
                Directory.CreateDirectory(StorageFolderPath);
            }

            var fileInfoDto = FileMapper.ConvertToFileInfoDTO(file);

            var fileInfo = await _db.Files.CreateAsync(FileMapper.ConvertToFileInfo(fileInfoDto));

            var filePath = GetAbsoluteFilePath(fileInfo);


            if (!File.Exists(filePath))
            {
                using (var fileStream = File.Create(filePath))
                {
                    using (data)
                    {
                        if (data.CanSeek)
                        {
                            data.Seek(0, SeekOrigin.Begin);
                        }

                        await data.CopyToAsync(fileStream);
                    }
                }
                await _db.SaveAsync(); // don't move to other place.
            }
            return await GetFileInfoAsync(fileInfo.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var file = await _db.Files.GetAsync(id);
            if (file == null)
            {
                return;
            }

            var post = await _db.Posts.GetAsync(file.PostFile.PostId);
            var currentUser = _userContext.CurrentUser;
            if (currentUser.RoleId!=(int)Roles.Moderator && post.UserId != currentUser.Id)
            {
                throw new UnauthorizedAccessException();
            }

            await _db.Files.DeleteAsync(file.Id);
            await _db.SaveAsync();

            var filePath = GetAbsoluteFilePath(file);
            File.Delete(filePath);
        }

        public async Task<FileInfoDTO> GetFileInfoAsync(Guid id)
        {
            return await GetFileFromDbAsync(id);
        }

        public async Task<Stream> GetFileContentAsync(Guid id)
        {
            var file = await _db.Files.GetAsync(id);
            if (file == null)
            {
                return null;
            }

            var filePath = GetAbsoluteFilePath(file);

            return new FileStream(filePath, FileMode.Open);
        }

        private string GetAbsoluteFilePath(FileInfo file)
        {
            var fileExtension = Path.GetExtension(file.Name);

            var fileName = file.Id + fileExtension;

            return Path.Combine(StorageFolderPath, fileName);
        }

        private async Task<FileInfoDTO> GetFileFromDbAsync(Guid id)
        {
            return FileMapper.ConvertToFileInfoDTO(await _db.Files.GetAsync(id));
        }

        public async Task<List<FileInfoDTO>> GetFilesForPost(int postId)
        {
            var files = await _db.Files.FindValidAsync(f => f.PostFile.PostId == postId);
            var filesDTO = files?.Select(FileMapper.ConvertToFileInfoDTO);
            return filesDTO.ToList();
        }
    }
}