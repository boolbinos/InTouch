using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;
using System.Linq.Expressions;

namespace Materialise.InTouch.BLL.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetCommentsAsync();

        Task<CommentDTO> GetCommentAsync(int id);

        Task<IEnumerable<CommentDTO>> GetCommentsByUserAsync(int userId);

        Task<IEnumerable<CommentDTO>> GetCommentsByPostAsync(int postId);

        Task<CommentDTO> CreateAsync(CommentDTO commentDTO);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<LatestCommentDTO>> GetLatestComments();

        Task<IEnumerable<CommentDTO>> FindValidAsync(Expression<Func<Comment, bool>> predicate);

    }
}
