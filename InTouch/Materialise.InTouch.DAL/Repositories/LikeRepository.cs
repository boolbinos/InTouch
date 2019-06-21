using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Materialise.InTouch.DAL.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Materialise.InTouch.DAL.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly InTouchContext _db;
        public LikeRepository(InTouchContext db)
        {
            _db = db;
        }

        public async Task AddLike(int postId, int userId)
        {
            await _db.Likes.AddAsync(new PostLike() { PostId = postId, UserId = userId });
        }

        public async Task RemoveLike(int postId, int userId)
        {
            var like = _db.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == userId);
            if (like == null)
                return;

            _db.Likes.Remove(like);
        }

        public async Task<List<User>> GetUsersLikesForPost(int postId)
        {
            var usersLikes = _db.Likes
                .Where(l => l.PostId == postId)
                .Include(l => l.User)
                .ThenInclude(u => u.Role);

            var result = (await usersLikes.ToListAsync()).Select(p=>p.User).ToList();
            return result;
        }
    }
}
