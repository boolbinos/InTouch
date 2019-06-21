using System;
using System.Collections.Generic;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class PostDTO
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

        public ICollection<CommentDTO> CommentsDTO { get; set; }
        public ICollection<FileInfoDTO> Files { get; set; }

        public ICollection<UserDTO> UsersLikes { get; set; }
        public PostDTO()
        {
            Files = new List<FileInfoDTO>();
            CommentsDTO = new List<CommentDTO>();
            UsersLikes = new List<UserDTO>();
        }
    }
}