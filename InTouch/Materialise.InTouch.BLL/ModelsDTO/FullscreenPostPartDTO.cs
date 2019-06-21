using System;
using System.Collections.Generic;
using Materialise.InTouch.BLL.Enumes;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class FullscreenPostPartDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public PostType PostType { get; set; }
        public PostPriority Priority { get; set; }
        public int DurationInSeconds { get; set; }
        public string VideoUrl { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public ContentType ContentType { get; set; }
        public FileInfoDTO File { get; set; }

    }
}