using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPublic { get; set; }
        public User User { get; set; }
        public PostType PostType { get; set; }
        public int DurationInSeconds { get; set; }
        public string VideoUrl { get; set; }
        public PostPriority Priority { get; set; }

        public int UserId { get; set; }
        public bool IsDeleted { get; set; }

        public int? ModifiedByUserId { get; set; }
        public User ModifiedByUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }


        public ICollection<Comment> Comments { get; set; }

        public ICollection<PostLike> PostLikes { get; set; }

        public virtual ICollection<PostFile> PostFiles { get; set; }

        public Post()
        {
            PostFiles = new List<PostFile>();
            PostLikes = new List<PostLike>();
            Comments = new List<Comment>();
        }

    }
}
