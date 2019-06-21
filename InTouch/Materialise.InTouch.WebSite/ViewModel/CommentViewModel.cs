using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.ViewModel
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool isDeleted { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }
        public byte[] Avatar { get; set; }
    }
}
