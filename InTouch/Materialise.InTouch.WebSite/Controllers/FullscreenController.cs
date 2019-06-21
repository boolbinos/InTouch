using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.WebSite.Mappers;
using Materialise.InTouch.WebSite.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Materialise.InTouch.WebSite.Controllers
{
    [Route("api/[controller]")]
    public class FullscreenController : Controller
    {
        private readonly IPostService _postService;
        private readonly IFullscreenBatchService _batchService;

        public FullscreenController(IPostService postService, IFullscreenBatchService batchService)
        {
            _postService = postService;
            _batchService = batchService;
        }

        [HttpGet("{lastBatchDate}/{lastPostCreateDate}")]
        public async Task<IActionResult> GetBatch(DateTime? lastBatchDate, DateTime? lastPostCreateDate)
        {
            var posts = await _postService.GetPostsForBatchAsync(lastBatchDate, lastPostCreateDate);

            if (!posts.Any())
            {
                return NoContent();
            }

            var batchDTO = await _batchService.CreateBatchAsync(posts);

            var modelCollection = PostMapper.ConvertToFullScreenPostPartViewModelCollection(batchDTO);

            return Ok(modelCollection);
        }

    }
}