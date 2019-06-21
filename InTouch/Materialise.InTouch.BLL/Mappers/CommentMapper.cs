using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.BLL.Mappers
{
    public static class CommentMapper
    {
        #region ToCommentDTO

        public static CommentDTO ConvertToCommentDTO(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            var commentDTO = new CommentDTO
            {
                Id = comment.Id,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Content = comment.Content,
                CreatedDate = comment.CreatedDate,
                isDeleted = comment.isDeleted,
            };

            return commentDTO;
        }

        public static Comment ConvertToComment(CommentDTO commentDTO)
        {
            if (commentDTO == null)
            {
                throw new ArgumentNullException(nameof(commentDTO));
            }

            var comment = new Comment
            {
                Id = commentDTO.Id,
                UserId = commentDTO.UserId,
                PostId = commentDTO.PostId,
                Content = commentDTO.Content,
                CreatedDate = commentDTO.CreatedDate,
                isDeleted = commentDTO.isDeleted,
            };

            return comment;
        }

        public static List<CommentDTO> ConvertToCommentDTOCollection(ICollection<Comment> comments)
        {
            if (comments == null)
            {
                throw new ArgumentNullException(nameof(comments));
            }

            var commentsDTO = comments.Select(ConvertToCommentDTO).ToList();

            return commentsDTO;
        }

        public static List<Comment> ConvertToCommentCollection(ICollection<CommentDTO> commentsDTO)
        {
            if (commentsDTO == null)
            {
                throw new ArgumentNullException(nameof(commentsDTO));
            }

            var comments = commentsDTO.Select(ConvertToComment).ToList();

            return comments;
        }

        #endregion

        #region ToLatestCommentDTO

        public static LatestCommentDTO ConvertToLatestCommentDTO(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            var commentDTO = new LatestCommentDTO
            {
                PostId = comment.PostId,
                PostTitle = comment.Post.Title,
                CommentId = comment.Id,
                Content = comment.Content,
                AuthorFirstName = comment.User.FirstName,
                AuthorLastName = comment.User.LastName,
                CreatedDate = comment.CreatedDate
            };
            return commentDTO;
        }
        #endregion
    }
}
