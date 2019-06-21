using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class LatestCommentDTO
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public int CommentId { get; set; }
        public string Content { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
