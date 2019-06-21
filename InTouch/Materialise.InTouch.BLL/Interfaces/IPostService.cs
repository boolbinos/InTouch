using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.BLL.Interfaces
{
    public interface IPostService
    {
        Task<PagedResultDTO<PostDTO>> GetPageAsync(int id, int page, int pageSize, bool justMyPosts, string srchStr = null);

        Task<PostDTO> GetAsync(int id);

        Task<List<PostDTO>> GetPostsForBatchAsync(DateTime? lastBatchDate, DateTime? lastPostCreateDate);

        Task<IEnumerable<PostDTO>> FindValidAsync(Expression<Func<Post, bool>> predicate);

        Task<bool> DeleteAsync(int id);

        Task<PostDTO> CreateAsync(PostDTO post);

        Task<PostDTO> Update(int postId, EditPostDTO post);

        Task<bool> DoPublic(int id);

        Task<bool> DoPrivate(int id);
    }
}