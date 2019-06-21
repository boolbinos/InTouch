using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedDate { get; set; }

        public UserDTO UserDTO { get; set; }
    }
}
