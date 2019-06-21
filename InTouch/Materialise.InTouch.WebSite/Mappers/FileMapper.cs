using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.BLL.Services.Storage.Models;
using Materialise.InTouch.WebSite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Mappers
{
    public class FileMapper
    {
        #region To FileService Requests
        public static CreateFileRequest ConvertToCreateFileRequest(FileInfoViewModel file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            var createFileRequest = new CreateFileRequest
            {
                Id = file.Id,
                ContentType = file.ContentType,
                Name = file.Name,
                SizeKB = file.SizeKB
            };
            return createFileRequest;
        }
        #endregion

        #region ToFileInfo
        public static FileInfoViewModel ConvertToFileInfoViewModel(FileInfoDTO file)
        {
            if (file==null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var fileInfoViewModel = new FileInfoViewModel()
            {
                Id = file.Id,
                Name = file.Name,
                ContentType = file.ContentType,
                SizeKB = file.SizeKB
            };

            return fileInfoViewModel;
        }
        public static List<FileInfoViewModel> ConvertToFileInfoViewModelCollection(ICollection<FileInfoDTO> filesDTO)
        {
            List<FileInfoViewModel> filesViewModel = filesDTO.Select(f => ConvertToFileInfoViewModel(f)).ToList();
            return filesViewModel;
        }
        #endregion
        #region ToFileInfoDTO
        public static FileInfoDTO ConvertToFileInfoDTO(FileInfoViewModel file)
        {
            if (file==null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var fileInfoDTO = new FileInfoDTO()
            {
                Id = file.Id,
                Name = file.Name,
                ContentType = file.ContentType,
                SizeKB = file.SizeKB
            };

            return fileInfoDTO;
        }
        public static ICollection<FileInfoDTO> ConvertToFileInfoDTOCollection(ICollection<FileInfoViewModel> files)
        {
            var fileInfoDTOs = files.Select(ConvertToFileInfoDTO).ToList();
            return fileInfoDTOs;
        }
        #endregion
    }
}
