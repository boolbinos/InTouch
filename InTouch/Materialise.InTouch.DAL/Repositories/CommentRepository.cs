using Materialise.InTouch.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Materialise.InTouch.DAL.Entities;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Materialise.InTouch.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Materialise.InTouch.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly InTouchContext db;

        public CommentRepository(InTouchContext context)
        {
            db = context;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await db.Comments
               .Include(u => u.User)
               .Include(p => p.Post)
               .OrderByDescending(c => c.CreatedDate)
               .ToListAsync();
        }

        public async Task<Comment> GetAsync(int id)
        {
            var comment = await db.Comments
                .Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == id);

            return comment;
        }

        public async Task<List<Comment>> GetAllByPost(int postId)
        {
            var comments = await db.Comments
                .Where(c => !c.isDeleted && c.PostId == postId)
                .Include(u => u.User)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return comments;
        }

        public async Task<List<Comment>> GetAllByUser(int userId)
        {
            var comments = await db.Comments.
                Where(c => !c.isDeleted && c.UserId == userId)
                .Include(p => p.Post)
                .Include(u => u.User)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return comments;
        }

        public async Task<Comment> CreateAsync(Comment item)
        {
            var createdComment = await db.Comments.AddAsync(item);
            return createdComment.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return false;
            }
            comment.isDeleted = true;
            return true;

        }

        public Task<List<Comment>> FindAsync(Expression<Func<Comment, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> FindValidAsync(Expression<Func<Comment, bool>> predicate)
        {
            return await db.Comments.Where(predicate)
                .Where(c => !c.isDeleted)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetAllValidAsync()
        {
            return await db.Comments
                .Include(u => u.User)
                .Include(p => p.Post)
                .Where(c => !c.isDeleted)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public Task<Comment> Update(Comment item)
        {
            throw new NotImplementedException();
        }
    }
}
