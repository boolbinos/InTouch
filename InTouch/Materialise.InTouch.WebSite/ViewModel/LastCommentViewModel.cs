using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.ViewModel
{
    public class LastCommentViewModel
    {
        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public int CommentId { get; set; }

        public string CommentContent { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
