using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Materialise.InTouch.WebSite.Model;
using System;

namespace Materialise.InTouch.WebSite.ViewModel
{
    public class PostEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int DurationInSeconds { get; set; }
        public string VideoUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Priority { get; set; }
        public byte[] Avatar { get; set; }

        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<FileInfoViewModel> Files { get; set; } = new List<FileInfoViewModel>();
    }
}
