using Materialise.InTouch.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class PostPageItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPublic { get; set; }
        public UserDTO UserDTO { get; set; }
        public PostType PostType { get; set; }
        public PostPriority Priority { get; set; }
        public int DurationInSeconds { get; set; }
        public string VideoUrl { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int UserId { get; set; }
        public bool IsDeleted { get; set; }

        public int CommentsCount { get; set; }
        public ICollection<FileInfoDTO> Files { get; set; }

        public ICollection<UserDTO> UsersLikes { get; set; }
        public PostPageItemDTO()
        {
            Files = new List<FileInfoDTO>();
            UsersLikes = new List<UserDTO>();
        }
    }
}
