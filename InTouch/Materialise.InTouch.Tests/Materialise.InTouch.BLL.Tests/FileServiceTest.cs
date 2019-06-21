using System;
using System.IO;
using System.Text;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.BLL.Services.Storage;
using Materialise.InTouch.BLL.Services.Storage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32.SafeHandles;
using Moq;
using Xunit;

namespace Materialise.InTouch.Tests.Materialise.InTouch.BLL.Tests
{
    public class FileServiceTest
    {
        private readonly IFileService _fileService;

        public FileServiceTest()
        {
            _fileService = new FileService();
        }
    }
}
