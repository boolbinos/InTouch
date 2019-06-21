using Materialise.InTouch.WebSite.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite
{
    public interface IFileManager
    {
        Task<FileInfoViewModel> SaveToTempFolder(IFormFile file);
        FileStream GetFileStream(FileInfoViewModel file);
        FileStream GetFileStream(string fileName);
    }
}
