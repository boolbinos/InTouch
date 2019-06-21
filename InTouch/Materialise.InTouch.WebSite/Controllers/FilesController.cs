using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.WebSite.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Materialise.InTouch.WebSite.Model;
using Microsoft.AspNetCore.StaticFiles;

namespace Materialise.InTouch.WebSite.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {

        private readonly IFileManager _fileManager;
        private readonly IFileService _fileService;

        public FilesController(IFileManager fileManager, IFileService fileService)
        {
            _fileManager = fileManager;
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var tempFile = await _fileManager.SaveToTempFolder(file);
            return Ok(tempFile);
        }

        [HttpGet]
        [Route("{fileId}/{fileName}")]
        public async Task<IActionResult> GetTempFile(Guid fileId, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);

            var stream = _fileManager.GetFileStream(fileId + fileExtension);

            var contentTypeProvider = new FileExtensionContentTypeProvider();
            var contentType = contentTypeProvider.Mappings[fileExtension];

            return new FileStreamResult(stream, contentType);
        }

        [HttpGet]
        [Route("{fileId}")]
        [ResponseCache(CacheProfileName = "CacheFiles")]
        public async Task<IActionResult> GetFile(Guid fileId)
        {
            var fileInfo = await _fileService.GetFileInfoAsync(fileId);
            var stream = await _fileService.GetFileContentAsync(fileId);
            return new FileStreamResult(stream, fileInfo.ContentType);
        }

        [HttpDelete]
        [Route("{fileId}")]
        public async Task<IActionResult> Delete(Guid fileId)
        {
            try
            {
                await _fileService.DeleteAsync(fileId);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}