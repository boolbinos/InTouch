using Materialise.InTouch.WebSite.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.ViewModel
{
    public class PostCreateViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int DurationInSeconds { get; set; }
        public string VideoUrl { get; set; }

        public ICollection<FileInfoViewModel> Files { get; set; } = new List<FileInfoViewModel>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Priority { get; set; }
    }
}
