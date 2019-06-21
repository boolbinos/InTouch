using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Model
{
    public class FileInfoViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public int SizeKB { get; set; }
        [NotMapped]
        public bool isAttached { get; set; }  //file is attached to existing file list in post edit
    }
}
