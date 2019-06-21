using Materialise.InTouch.WebSite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.WebSite.ViewModel
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPublic { get; set; }
        public string UserName { get; set; }
        public int DurationInSeconds { get; set; }
        public string VideoUrl { get; set; }
        public int UserId { get; set; }
        public string PostType { get; set; }
        public DateTime StartDate { get;set; }
        public DateTime EndDate { get; set; }
        public byte[] Avatar { get; set; }

        public string Priority { get; set; }
        public int CommentsAmount { get; set; }   
        public ICollection<FileInfoViewModel> Files { get; set; } = new List<FileInfoViewModel>();
        public ICollection<UserLikeViewModel> UsersLikes { get; set; } = new List<UserLikeViewModel>();
    }
}
