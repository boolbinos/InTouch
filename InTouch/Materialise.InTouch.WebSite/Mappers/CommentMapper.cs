using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.WebSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Mappers
{
    public class CommentMapper
    {
        #region CommentViewModel
        public static CommentViewModel ConvertToCommentViewModel(CommentDTO commentDTO)
        {
            if (commentDTO == null)
            {
                throw new ArgumentNullException();
            }

            var commentViewModel = new CommentViewModel()
            {
                Id = commentDTO.Id,
                UserId = commentDTO.UserId,
                PostId = commentDTO.PostId,
                Content = commentDTO.Content,
                CreatedDate = commentDTO.CreatedDate,
                isDeleted = commentDTO.isDeleted,
                UserFirstName = commentDTO.UserDTO.FirstName,
                UserLastName = commentDTO.UserDTO.LastName,
                Avatar=commentDTO.UserDTO.Avatar
            };

            return commentViewModel;
        }

        public static List<CommentViewModel> ConvertToCommentViewModelCollection(IEnumerable<CommentDTO> commentsDTO)
        {
            if (commentsDTO == null)
            {
                throw new ArgumentException();
            }

            List<CommentViewModel> commentsViewModel = new List<CommentViewModel>();

            foreach (var commentDTO in commentsDTO)
            {
                commentsViewModel.Add(ConvertToCommentViewModel(commentDTO));
            }

            return commentsViewModel;
        }

        public static CommentDTO ConvertToCommentDTO(CommentCreateModel createModel)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException();
            }

            CommentDTO commentDTO = new CommentDTO
            {
                PostId = createModel.PostId,
                Content = createModel.Content
            };

            return commentDTO;
        }
        #endregion

        #region LatestCommentViewModel

        public static LastCommentViewModel ConvertToLastCommentViewModel(LatestCommentDTO lastCommentDTO)
        {
            if (lastCommentDTO == null)
            {
                throw new ArgumentNullException();
            }
            return new LastCommentViewModel
            {
                PostId = lastCommentDTO.PostId,
                PostTitle = lastCommentDTO.PostTitle,
                CommentId = lastCommentDTO.CommentId,
                AuthorFirstName = lastCommentDTO.AuthorFirstName,
                AuthorLastName = lastCommentDTO.AuthorLastName,
                CommentContent = lastCommentDTO.Content,
                CreatedDate = lastCommentDTO.CreatedDate
            };
        }

        public static List<LastCommentViewModel> ConvertToLastCommentViewModelCollection(IEnumerable<LatestCommentDTO> latestCommentsDTO)
        {
            if (latestCommentsDTO == null)
            {
                throw new ArgumentException();
            }

            var latestCommentsViewModel = new List<LastCommentViewModel>();

            foreach (var latestCommentDTO in latestCommentsDTO)
            {
                latestCommentsViewModel.Add(ConvertToLastCommentViewModel(latestCommentDTO));
            }

            return latestCommentsViewModel;
        }

        #endregion
    }
}
