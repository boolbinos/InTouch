using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.Entities
{
    public class PostFile
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Guid FileId { get; set; }
        public bool IsDeleted { get; set; }

        public Post Post { get; set; }
        public FileInfo File { get; set; }

    }
}
