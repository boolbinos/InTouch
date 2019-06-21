using System;
using System.Collections.Generic;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.DAL.Repositories
{
    public class PostPageItem
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsPublic { get; set; }
        public User User { get; set; }
        public PostType PostType { get; set; }
        public int DurationInSeconds { get; set; }
        public PostPriority Priority { get; set; }
        public string VideoUrl { get; set; }
        public int UserId { get; set; }
        public User ModifiedByUser { get; set; }
        public int? ModifiedByUserId { get; set; }
        public ICollection<PostFile> PostFiles { get; set; }
        public ICollection<PostLike> PostLikes { get; set; }
        public int CommentsCount { get; set; }
    }
}