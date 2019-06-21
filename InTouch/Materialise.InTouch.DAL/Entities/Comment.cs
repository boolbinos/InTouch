using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public Post Post { get; set; }
        public User User { get; set; }
        public bool isDeleted { get; set; }
    }
}
