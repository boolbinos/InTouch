using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.WebSite.Extensions;
using Materialise.InTouch.WebSite.Mappers;
using Materialise.InTouch.WebSite.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Materialise.InTouch.WebSite.Services.EmailService;
using Microsoft.AspNetCore.Hosting;
using System.Threading;

namespace Materialise.InTouch.WebSite.Controllers
{
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUserContext _userContext;
        private readonly IEmailNotificationSender _emailSender;
        private readonly IHostingEnvironment _env;
        private readonly IPostService _postService;
        public CommentController(ICommentService commentService,
            IUserContext userContext, IEmailNotificationSender emailSender, IHostingEnvironment env, IPostService postService)
        {
            _commentService = commentService;
            _userContext = userContext;
            _emailSender = emailSender;
            _env = env;
            _postService = postService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CommentCreateModel commentCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (commentCreateModel == null)
            {
                return BadRequest();
            }

            if (commentCreateModel.Content.Length > 8000)
            {
                return BadRequest();
            }

            var comment = CommentMapper.ConvertToCommentDTO(commentCreateModel);
            var createdComment = await _commentService.CreateAsync(comment);
            var commentViewModel = CommentMapper.ConvertToCommentViewModel(createdComment);


            var post = await _postService.GetAsync(comment.PostId);
            var commentators = (await _commentService.FindValidAsync(c =>
            c.PostId == comment.PostId &&
            c.User.Email != post.UserDTO.Email &&
            c.UserId != comment.UserId))
            .Select(c => new CommentEmailModel()
            {
                
                Email= c.UserDTO.Email,
                FirstName = c.UserDTO.FirstName,
                LastName = c.UserDTO.LastName
            }).Distinct().ToList();

            _emailSender.PostCommentNotificationAsync(commentViewModel, post, commentators);
            return Ok(commentViewModel);
        }

        [HttpGet]
        [Route("LatestComments")]
        public async Task<IActionResult> GetLatestAsync()
        {
            var latestComments = await _commentService.GetLatestComments();
            var latestCommentsViewModel = CommentMapper.ConvertToLastCommentViewModelCollection(latestComments);
            return Ok(latestCommentsViewModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentService.GetCommentAsync(id);
            if (comment == null)
            {
                return BadRequest(404);
            }

            if (!HasCurrentUserRights(comment))
            {
                return StatusCode(403);
            }

            if (await _commentService.DeleteAsync(id))
            {
                return Ok("Comment Is Deleted");
            }

            return BadRequest(404);
        }

        private bool HasCurrentUserRights(CommentDTO comment)
        {
            return _userContext.CurrentUser.IsModerator() || _userContext.CurrentUser.Id == comment.UserId;
        }
    }
}
