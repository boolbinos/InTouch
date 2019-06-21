using Materialise.InTouch.BLL.Enumes;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.WebSite.Mappers;
using Materialise.InTouch.WebSite.Services.EmailService;
using Materialise.InTouch.WebSite.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Materialise.InTouch.WebSite.Extensions;
using Materialise.InTouch.WebSite.Model;

namespace Materialise.InTouch.WebSite.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller 
    {
        private readonly IFileManager _fileManager;
        private readonly IFileService _fileService;
        private readonly IPostService _postService;
        private readonly IExternalPostService _externalPostService;
        private readonly IUserContext _userContext;
        private readonly IEmailNotificationSender _emailService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;

        public PostsController(IPostService postService, IExternalPostService externalPostService,
            IFileService fileService, IFileManager fileManager, IUserContext userContext, IEmailNotificationSender emailService,
            ILikeService likeService, ICommentService commentService)
        {
            _postService = postService;
            _externalPostService = externalPostService;
            _fileManager = fileManager;
            _fileService = fileService;
            _userContext = userContext;
            _emailService = emailService;
            _commentService = commentService;
            _likeService = likeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPageAsync(InputPageData inputData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Count of posts must be less than 100 and more than 0");
            }

            int currentUserId = _userContext.CurrentUser.Id;

            var pagedResultDto = await _postService.GetPageAsync(currentUserId, inputData.PageNum, inputData.PageSize, inputData.JustMyPosts, inputData.SearchStr);
                    
            var posts = new List<PostViewModel>();

            pagedResultDto.Data.ForEach((p) =>
            {
                var filesViewModelCollection = p.Files.Select(FileMapper.ConvertToFileInfoViewModel);
                var usersLikes = p.UsersLikes.Select(UserMapper.ConvertToUserLikeViewModel);
                var postViewModel = PostMapper.ConvertToPostViewModel(p);
                postViewModel.Files = filesViewModelCollection.ToList();
                postViewModel.UsersLikes = usersLikes.ToList();
                posts.Add(postViewModel);
            });

            return Ok(new
            {
                Paging = pagedResultDto.Paging,
                Data = posts
            });
        }

      

        [HttpPost]
        [Route("Import/{providerName}")]
        [Authorize(Policy = "RequireModeratorRole")]
        public async Task<IActionResult> Import(string providerName)
        {
            var importedPostsCount = 0;

            if (string.IsNullOrEmpty(providerName))
                return BadRequest("Empty provider name");
            try
            {
                importedPostsCount = await _externalPostService.ImportPostsAsync(providerName);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(importedPostsCount);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostDetailAsync(int id)
        {
            var post = await _postService.GetAsync(id);

            if (post.IsDeleted || post == null)
            {
                return StatusCode(404);
            }

            var postDetailsViewModel = PostMapper.ConvertToPostDetailsViewModel(post);

            return Ok(postDetailsViewModel);
        }

        [HttpGet]
        [Route("GetPostEdit/{id}")]
        public async Task<IActionResult> GetPostEditViewModelAsync(int id)
        {            
            var post = await _postService.GetAsync(id);

            if (!HasCurrentUserEditRights(post))
            {
                return StatusCode(403);
            }

            if (post.IsDeleted || post == null)
            {
                return StatusCode(404);
            }
            var files = await _fileService.GetFilesForPost(post.Id);
            var postEditViewModel = PostMapper.ConvertToPostEditModel(post);
            postEditViewModel.Files = files.Select(FileMapper.ConvertToFileInfoViewModel).ToList();
            return Ok(postEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] PostCreateViewModel postCreateModel)
        {
            if (postCreateModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            List<FileInfoDTO> filesDTO = new List<FileInfoDTO>();

            postCreateModel.VideoUrl = string.IsNullOrEmpty(postCreateModel.VideoUrl) ? null : postCreateModel.VideoUrl;

            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            postCreateModel.StartDate = postCreateModel.StartDate.AddHours(offset.TotalHours);
            postCreateModel.EndDate = postCreateModel.EndDate.AddHours(offset.TotalHours);

            foreach (var file in postCreateModel.Files)
            {
                var createRequestFile = FileMapper.ConvertToCreateFileRequest(file);

                using (var stream = _fileManager.GetFileStream(file))
                {
                    var createdPostFileDTO = await _fileService.CreateAsync(createRequestFile, stream);
                    filesDTO.Add(createdPostFileDTO);
                }
            }

            var post = PostMapper.ConvertToPostDTO(postCreateModel);
            post.Files = filesDTO;
            var createdPostDTO = await _postService.CreateAsync(post);

            var currentUser = _userContext.CurrentUser;
            var postViewModel = PostMapper.ConvertToPostViewModel(createdPostDTO);
            if (currentUser.RoleId != (int)Roles.Moderator)
            {
                _emailService.PostCreatedNotificationAsync(postViewModel);
            }
            return Ok(createdPostDTO);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PostEditViewModel postEditModel)
        {
            if (postEditModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var post = await _postService.GetAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (!HasCurrentUserEditRights(post))
            {
                return StatusCode(403);
            }
            
            var editPostDto = MapToEditPostDTO(postEditModel);
            var updatedPost = await _postService.Update(id, editPostDto);

            var postViewModel = PostMapper.ConvertToPostViewModel(updatedPost);

            return Ok(postViewModel);
        }
      
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetAsync(id);
            if (post == null)
            {
                return BadRequest(404);
            }

            if (!HasCurrentUserEditRights(post))
            {
                return StatusCode(403);
            }

            if (await _postService.DeleteAsync(id))
            {
                return Ok("Post Is Deleted");
            }

            return BadRequest(404);
        }

        [HttpPut]
        [Route("{id}/Public")]
        [Authorize(Policy = "RequireModeratorRole")]
        public async Task<IActionResult> DoPublicPost(int id)
        {
            var result = await _postService.DoPublic(id);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/Private")]
        [Authorize(Policy = "RequireModeratorRole")]
        public async Task<IActionResult> DoPrivatePost(int id)
        {
            var result = await _postService.DoPrivate(id);
            return Ok(result);
        }


        [HttpGet]
        [Route("{id}/comments")]
        public async Task<IActionResult> GetCommentsForPost(int id)
        {
            var comments = await _commentService.GetCommentsByPostAsync(id);
            var commentsView = comments.Select(CommentMapper.ConvertToCommentViewModel);
            return Ok(commentsView);
        }

        [HttpGet]
        [Route("{id}/files")]
        public async Task<IActionResult> GetFilesForPost(int id)
        {
            var files = await _fileService.GetFilesForPost(id);
            var filesInfoView = files.Select(FileMapper.ConvertToFileInfoViewModel);
            return Ok(filesInfoView);
        }

        [HttpGet("{id}/likes")]
        public async Task<IActionResult> GetLikesForPost(int id)
        {
            var likes = await _likeService.GetUsersLikes(id);
            var usersLikes = likes.Select(UserMapper.ConvertToUserLikeViewModel);
            return Ok(usersLikes);
        }


        private bool HasCurrentUserEditRights(PostDTO post)
        {
            return _userContext.CurrentUser.IsModerator() || _userContext.CurrentUser.Id == post.UserId;
        }
        private EditPostDTO MapToEditPostDTO(PostEditViewModel postEditModel)
        {
            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            postEditModel.StartDate = postEditModel.StartDate.AddHours(offset.TotalHours);
            postEditModel.EndDate = postEditModel.EndDate.AddHours(offset.TotalHours);

            var editPostDto = PostMapper.ConvertToEditPostDTO(postEditModel);            
            editPostDto.Files = MapFilesToDto(postEditModel.Files);

            return editPostDto;
        }
        private List<FileInfoDTO> MapFilesToDto(IEnumerable<FileInfoViewModel> files)
        {
            return files
                .Select(file =>
                {
                    if (file.isAttached)
                    {
                        var newFile = FileMapper.ConvertToCreateFileRequest(file);
                        return _fileService.CreateAsync(newFile, _fileManager.GetFileStream(file)).Result;
                    }
                    else
                    {
                        return FileMapper.ConvertToFileInfoDTO(file);
                    }
                })
                .ToList();
        }

    }
}