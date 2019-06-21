using Materialise.InTouch.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Materialise.InTouch.DAL.Interfaces
{
    public interface ILikeRepository
    {
        Task<List<User>> GetUsersLikesForPost(int postId);
        Task AddLike(int postId, int userId);
        Task RemoveLike(int postId, int userId);
    }
}
