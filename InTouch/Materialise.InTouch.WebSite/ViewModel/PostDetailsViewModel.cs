using Materialise.InTouch.WebSite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.ViewModel
{
    public class PostDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string VideoUrl { get; set; }
        public int UserId { get; set; }
        public bool? IsPublic { get; set; }
        public string PostType { get; set; }
        public string Priority { get; set; }
        public byte[] Avatar { get; set; }
    }
}
