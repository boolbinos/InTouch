using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Model
{
    public class FileManager : IFileManager
    {
        private string _storagePath;

        public FileManager(IHostingEnvironment env)
        {
            _storagePath = Path.Combine(env.ContentRootPath, "Uploads");
        }

        public async Task<FileInfoViewModel> SaveToTempFolder(IFormFile file)
        {
            if (file.Length > 0)
            {
                if (!Directory.Exists(_storagePath))
                    Directory.CreateDirectory(_storagePath);

                FileInfoViewModel createdFile = new FileInfoViewModel()
                {
                    Name = file.FileName,
                    ContentType = file.ContentType,
                    Id = Guid.NewGuid(),
                    SizeKB = (int) (file.Length / 1000)
                };

                var saveFileName = createdFile.Id.ToString() + '.' + createdFile.Name.Split('.').Last();
                var savePath = _storagePath + "\\" + saveFileName;

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return createdFile;
            }
            throw new ArgumentException("File length should be more then zero");
        }

        public FileStream GetFileStream(FileInfoViewModel file)
        {
            var fileName = file.Id + Path.GetExtension(file.Name);
            return GetFileStream(fileName);
        }

        public FileStream GetFileStream(string fileName)
        {
            var filePath = _storagePath + "\\" + fileName;

            FileStream stream = null;
            if (File.Exists(filePath))
            {
                stream = new FileStream(filePath, FileMode.Open);
            }

            return stream;
        }
    }
}