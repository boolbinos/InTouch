using System;
using System.IO;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class FileInfoDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public int SizeKB{ get; set; }
    }
}