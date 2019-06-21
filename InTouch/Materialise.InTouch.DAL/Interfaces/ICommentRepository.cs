using Materialise.InTouch.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Materialise.InTouch.DAL.Interfaces
{
    public interface ICommentRepository : IRepository<Comment, int>
    {
        Task<List<Comment>> GetAllByPost(int postId);
        Task<List<Comment>> GetAllByUser(int userId);
    }
}
