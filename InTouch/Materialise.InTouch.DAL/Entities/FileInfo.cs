using System;
using System.Collections.Generic;

namespace Materialise.InTouch.DAL.Entities
{
    public class FileInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public int SizeKB { get; set; }
        public bool IsDeleted { get; set; }
        public virtual PostFile PostFile { get; set; }
    }
}