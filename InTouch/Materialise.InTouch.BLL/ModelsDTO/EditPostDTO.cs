using System.Collections.Generic;
using Materialise.InTouch.DAL.Entities;
using System;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class EditPostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int DurationInSeconds { get; set; }
        public string VideoUrl { get; set; }
        public PostPriority Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<FileInfoDTO> Files { get; set; }
    }
}
