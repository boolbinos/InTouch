using System;
using System.IO;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.BLL.Services.Storage.Models;
using System.Collections.Generic;

namespace Materialise.InTouch.BLL.Interfaces
{
    public interface IFileService
    {
        Task<FileInfoDTO> CreateAsync(CreateFileRequest file, Stream data);
        Task DeleteAsync(Guid id);
        Task<FileInfoDTO> GetFileInfoAsync(Guid id);
        Task<Stream> GetFileContentAsync(Guid id);
        Task<List<FileInfoDTO>> GetFilesForPost(int postId);
    }
}