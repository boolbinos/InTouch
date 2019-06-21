using System;
using System.Collections.Generic;
using System.Linq;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.BLL.Services.Storage.Models;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.BLL.Mappers
{
    public class FileMapper
    {
        #region To FileService Requests

        public static CreateFileRequest ConvertToCreateFileRequest(FileInfoDTO file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            var createFileRequest = new CreateFileRequest
            {
                ContentType = file.ContentType,
                Name = file.Name,
                SizeKB = file.SizeKB
            };
            return createFileRequest;
        }


        #endregion

        #region To FileInfoDTO

        public static FileInfoDTO ConvertToFileInfoDTO(CreateFileRequest createRequest)
        {
            if (createRequest == null)
            {
                throw new ArgumentNullException();
            }
            return new FileInfoDTO
            {
                Name = createRequest.Name,
                Id = createRequest.Id,
                SizeKB = createRequest.SizeKB,
                ContentType = createRequest.ContentType
            };
        }

        public static FileInfoDTO ConvertToFileInfoDTO(PostFile postFile)
        {
            if (postFile == null)
            {
                throw new ArgumentNullException();
            }
            var file = postFile.File;
            return new FileInfoDTO
            {
                Name = file.Name,
                Id = file.Id,
                SizeKB = file.SizeKB,
                ContentType = file.ContentType
            };
        }

        public static FileInfoDTO ConvertToFileInfoDTO(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException();
            }
            return new FileInfoDTO
            {
                Name = fileInfo.Name,
                Id = fileInfo.Id,
                SizeKB = fileInfo.SizeKB,
                ContentType = fileInfo.ContentType
            };
        }

        public static ICollection<FileInfoDTO> ConvertToFileInfoDTOCollection(ICollection<PostFile> postFiles)
        {
            if (postFiles == null)
            {
                throw new ArgumentNullException();
            }
            return postFiles.Select(ConvertToFileInfoDTO).ToList();
        }

        #endregion

        #region To FileInfo

        public static FileInfo ConvertToFileInfo(FileInfoDTO fileInfoDto)
        {
            if (fileInfoDto == null)
                throw new ArgumentNullException();

            return new FileInfo
            {
                Id = fileInfoDto.Id,
                Name = fileInfoDto.Name,
                ContentType = fileInfoDto.ContentType,
                SizeKB = fileInfoDto.SizeKB
            };
        }

        public static ICollection<FileInfo> ConvertToFileInfoCollection(ICollection<FileInfoDTO> postDTOFiles)
        {
            if (postDTOFiles == null)
                throw new ArgumentNullException();
            return postDTOFiles.Select(f => ConvertToFileInfo(f)).ToList();
        }

        #endregion

        #region To PostFile

        public static PostFile ConvertToPostFile(FileInfoDTO fileDTO)
        {
            var postFile = new PostFile
            {
                File = new FileInfo
                {
                    Id = fileDTO.Id,
                    Name = fileDTO.Name,
                    ContentType = fileDTO.ContentType,
                    SizeKB = fileDTO.SizeKB
                }
            };
            return postFile;
        }

        public static List<PostFile> ConvertToPostFileCollection(ICollection<FileInfoDTO> filesDTO)
        {
            var postFiles = filesDTO.Select(f => ConvertToPostFile(f)).ToList();
            return postFiles;
        }

        #endregion
    }
}