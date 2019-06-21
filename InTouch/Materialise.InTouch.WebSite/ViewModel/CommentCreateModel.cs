using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.ViewModel
{
    public class CommentCreateModel
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
