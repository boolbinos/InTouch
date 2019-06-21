using System;

namespace Materialise.InTouch.BLL.Services.Storage.Models
{
    public class CreateFileRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public int SizeKB { get; set; }
    }
}