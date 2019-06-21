using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.WebSite.Mappers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Controllers
{
    [Route("api/[controller]")]
    public class LikesController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserContext _userContext;
        private readonly ILikeService _likeService;
        public LikesController(IPostService postService, IUserContext userContext, ILikeService likeService)
        {
            _postService = postService;
            _userContext = userContext;
            _likeService = likeService;
        }
        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody]int postId)
        {
            //var result = await _postService.Like(postId, _userContext.CurrentUser.Id);
            var result = await _likeService.addLike(postId, _userContext.CurrentUser.Id);
            return Ok(result);
        }
    }
}
