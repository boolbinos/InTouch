using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.WebSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Enumes;

namespace Materialise.InTouch.WebSite.Mappers
{
    public class PostMapper
    {
        public static EditPostDTO ConvertToEditPostDTO(PostEditViewModel postEditModel)
        {
            if (postEditModel == null)
            {
                throw new ArgumentNullException(nameof(postEditModel));
            }

            return new EditPostDTO
            {
                Title = postEditModel.Title,
                Content = postEditModel.Content,
                DurationInSeconds = postEditModel.DurationInSeconds,
                VideoUrl = string.IsNullOrEmpty(postEditModel.VideoUrl) ? null : postEditModel.VideoUrl,
                StartDate = postEditModel.StartDate,
                EndDate = postEditModel.EndDate,
                Priority = (PostPriority)Enum.Parse(typeof(PostPriority), postEditModel.Priority)
                
            };
        }

        public static PostEditViewModel ConvertToPostEditModel(PostDTO postDTO)
        {
            if (postDTO == null)
            {
                throw new ArgumentNullException(nameof(postDTO));
            }

            var postEditModel = new PostEditViewModel()
            {
                Id = postDTO.Id,
                Title = postDTO.Title,
                Content = postDTO.Content,
                DurationInSeconds = postDTO.DurationInSeconds,
                Priority = postDTO.Priority.ToString(),
                StartDate = postDTO.StartDateTime,
                EndDate = postDTO.EndDateTime,
                VideoUrl = postDTO.VideoUrl,
                CreatedDate = postDTO.CreatedDate,
                UserName = postDTO.UserDTO.FirstName +' '+ postDTO.UserDTO.LastName,
                Avatar = postDTO.UserDTO.Avatar
            };

            return postEditModel;
        }
        public static PostDTO ConvertToPostDTO(PostCreateViewModel createModel)
        {
            if (createModel == null)
                throw new ArgumentNullException();

            PostDTO postDTO = new PostDTO()
            {
                Title = createModel.Title,
                Content = createModel.Content,
                DurationInSeconds = createModel.DurationInSeconds,
                VideoUrl = createModel.VideoUrl,
                StartDateTime = createModel.StartDate,
                EndDateTime = createModel.EndDate,
                Priority = (PostPriority)Enum.Parse(typeof(PostPriority), createModel.Priority)
            };

            return postDTO;
        }

        #region ToPostViewModel
        public static List<PostViewModel> ConvertToPostViewModelCollection(IEnumerable<PostDTO> postsDTO)
        {
            if (postsDTO == null)
                throw new ArgumentNullException();

            List<PostViewModel> postsViewModel = postsDTO.Select(p => ConvertToPostViewModel(p)).ToList();

            return postsViewModel;
        }

        public static PostDetailsViewModel ConvertToPostDetailsViewModel(PostDTO postDTO)
        {
            if (postDTO == null)
                throw new ArgumentNullException();

            var postDetailsViewModel = new PostDetailsViewModel()
            {
                Id = postDTO.Id,
                Title = postDTO.Title,
                Content = postDTO.Content,
                CreatedDate = postDTO.CreatedDate,
                IsPublic = postDTO.IsPublic,
                UserName = postDTO.UserDTO.FirstName + " " + postDTO.UserDTO.LastName,
                UserId = postDTO.UserId,
                VideoUrl = postDTO.VideoUrl,
                PostType = postDTO.PostType.ToString(),
                Priority = postDTO.Priority.ToString(),
                Avatar = postDTO.UserDTO.Avatar
            };
            return postDetailsViewModel;
        }

        public static PostViewModel ConvertToPostViewModel(PostDTO postDTO)
        {
            if (postDTO == null)
                throw new ArgumentNullException();

            var postViewModel = new PostViewModel()
            {
                Id = postDTO.Id,
                Title = postDTO.Title,
                Content = postDTO.Content,
                CreatedDate = postDTO.CreatedDate,
                IsPublic = postDTO.IsPublic,
                UserName = postDTO.UserDTO.FirstName + " " + postDTO.UserDTO.LastName,
                UserId = postDTO.UserId,
                VideoUrl = postDTO.VideoUrl,
                DurationInSeconds = postDTO.DurationInSeconds,
                PostType = postDTO.PostType.ToString(),
                Priority = postDTO.Priority.ToString(),
                StartDate = postDTO.StartDateTime,
                EndDate = postDTO.EndDateTime,
                CommentsAmount = postDTO.CommentsDTO.Count,
                Avatar = postDTO.UserDTO.Avatar
            };
            return postViewModel;
        }
        public static FullScreenPostPartViewModel ConvertToFullScreenPostPartViewModel(
            FullscreenPostPartDTO postPartDTO)
        {
            if (postPartDTO == null)
                throw new ArgumentNullException();

            var postViewModel = new FullScreenPostPartViewModel()
            {
                Id = postPartDTO.Id,
                Title = postPartDTO.Title,
                ContentType = postPartDTO.ContentType,
                CreatedDate = postPartDTO.CreatedDate,
                VideoUrl = postPartDTO.VideoUrl,
                File = postPartDTO.File == null ? null : FileMapper.ConvertToFileInfoViewModel(postPartDTO.File),
                DurationInSeconds = postPartDTO.DurationInSeconds,
                PostType = postPartDTO.PostType.ToString(),
                Priority = postPartDTO.Priority.ToString()                 
            };
            return postViewModel;
        }

        public static List<FullScreenPostPartViewModel> ConvertToFullScreenPostPartViewModelCollection(
            List<FullscreenPostPartDTO> postPartDTOs)
        {
            return postPartDTOs.Select(ConvertToFullScreenPostPartViewModel).ToList();
        }

        #endregion
    }
}
