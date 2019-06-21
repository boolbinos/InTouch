using System;
using System.Collections.Generic;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;
using System.Linq;
using Materialise.InTouch.BLL.Enumes;

namespace Materialise.InTouch.BLL.Mappers
{
    public static class PostMapper
    {
        #region ToPostDTO
        public static PostDTO ConvertToPostDTO(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            var postDTO = new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                IsPublic = post.IsPublic,
                UserId = post.UserId,
                DurationInSeconds = post.DurationInSeconds,
                IsDeleted = post.IsDeleted,
                PostType = post.PostType,
                VideoUrl = post.VideoUrl,
                Files = post.PostFiles == null ? null : FileMapper.ConvertToFileInfoDTOCollection(post.PostFiles),
                StartDateTime = post.StartDateTime,
                EndDateTime = post.EndDateTime,
                Priority = post.Priority,
                UserDTO = post.User == null ? null : UserMapper.ConvertToUserDTO(post.User)
            };

            return postDTO;
        }
        public static FullscreenPostPartDTO ConvertToFullscreenPostPartWithNoContent(PostDTO post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            
            return new FullscreenPostPartDTO
            {
                Id = post.Id,
                Priority = post.Priority,
                PostType = post.PostType,
                ContentType = ContentType.None, 
                CreatedDate = post.CreatedDate,
                StartDateTime = post.StartDateTime,
                EndDateTime = post.EndDateTime
            };
        }
        public static List<PostDTO> ConvertToPostDTOCollection(ICollection<Post> posts)
        {
            if (posts == null)
            {
                throw new ArgumentNullException(nameof(posts));
            }

            var postsDTO = posts.Select(ConvertToPostDTO).ToList();

            return postsDTO;
        }
        #endregion
        #region ToPost
        public static Post ConvertToPost(PostDTO postDTO)
        {
            if (postDTO == null)
            {
                throw new ArgumentNullException(nameof(postDTO));
            }

            var post = new Post
            {
                Id = postDTO.Id,
                Title = postDTO.Title,
                Content = postDTO.Content,
                CreatedDate = postDTO.CreatedDate,
                IsPublic = postDTO.IsPublic,
                PostType = postDTO.PostType,
                UserId = postDTO.UserId,
                DurationInSeconds = postDTO.DurationInSeconds,
                VideoUrl=postDTO.VideoUrl,
                IsDeleted = postDTO.IsDeleted,
                StartDateTime = postDTO.StartDateTime,
                EndDateTime = postDTO.EndDateTime,
                Priority = postDTO.Priority
            };

            return post;
        }
        public static List<Post> ConvertToPostCollection(ICollection<PostDTO> postsDTO)
        {
            if (postsDTO == null)
            {
                throw new ArgumentNullException(nameof(postsDTO));
            }

            var posts = postsDTO.Select(ConvertToPost).ToList();

            return posts;
        }
#endregion
    }
}