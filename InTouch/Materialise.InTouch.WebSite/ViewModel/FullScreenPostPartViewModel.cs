using System;
using Materialise.InTouch.BLL.Enumes;
using Materialise.InTouch.WebSite.Model;

namespace Materialise.InTouch.WebSite.ViewModel
{
    public class FullScreenPostPartViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public ContentType ContentType { get; set; }
        public FileInfoViewModel File { get; set; }
        public string VideoUrl { get; set; }
        public int DurationInSeconds { get; set; }
        public string PostType { get; set; }
        public string Priority { get; set; }
    }
}