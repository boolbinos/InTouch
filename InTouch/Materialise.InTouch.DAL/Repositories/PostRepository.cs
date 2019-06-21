using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.DAL.Infrastructure;
using Materialise.InTouch.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Materialise.InTouch.DAL.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly InTouchContext db;

        public PostRepository(InTouchContext context)
        {
            db = context;
        }

        public async Task<List<Post>> GetAllValidAsync()
        {
            var posts = await db.Posts
                .Include(c => c.Comments)
                .ThenInclude(c => c.User)
                .ThenInclude(u => u.Role)
                .Include(p => p.User)
                .Include(f => f.PostFiles)
                .ThenInclude(pf => pf.File)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            return posts;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            var posts = await db.Posts
                .Include(c => c.Comments)
                .ThenInclude(c => c.User)
                .ThenInclude(u => u.Role)
                .Include(p => p.User)
                .Include(f => f.PostFiles)
                .ThenInclude(pf => pf.File)
                .Where(o => !o.IsDeleted)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            return posts;
        }

        public async Task<PagedResult<Post>> GetPageAsync(int id, int page, int pageSize, bool justMyPosts, string srchStr = null)
        {
            var currentDate = DateTime.Now;
            int skip = (page - 1) * pageSize;

            var query = db.Posts
                .Where(p => !p.IsDeleted);

            if (justMyPosts)
            {
                query = query.Where(p => p.UserId == id);
            }

            if (srchStr != null)
            {
                query = query.Where(p => p.Title.ToUpper()
                   .Contains(srchStr.ToUpper())
                   || p.Content.ToUpper()
                   .Contains(srchStr.ToUpper())).Distinct();
            }

            var count = query.Count();

            var posts = query
                .Include(c => c.Comments)
                .ThenInclude(c => c.User)
                .ThenInclude(u => u.Role)
                .Include(p => p.User)
                .Include(f => f.PostFiles)
                .ThenInclude(pf => pf.File)
                .Include(p => p.User.Role)
                .Include(p => p.PostLikes)
                .ThenInclude(pl => pl.User)
                .ThenInclude(u => u.Role)
                .OrderByDescending(p => (
                    currentDate > p.StartDateTime
                    && currentDate < p.EndDateTime
                    && p.Priority == PostPriority.High)
                    ? p.Priority
                    : 0)
                .ThenByDescending(p => p.CreatedDate)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new Post()
                {
                    Id = x.Id,
                    IsDeleted = x.IsDeleted,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedDate = x.CreatedDate,
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime,
                    IsPublic = x.IsPublic,
                    User = x.User,
                    PostType = x.PostType,
                    DurationInSeconds = x.DurationInSeconds,
                    Priority = x.Priority,
                    VideoUrl = x.VideoUrl,
                    UserId = x.UserId,
                    ModifiedByUser = x.ModifiedByUser,
                    ModifiedByUserId = x.ModifiedByUserId,
                    PostFiles = x.PostFiles,
                    PostLikes = x.PostLikes,
                    Comments = x.Comments.Where(c => !c.isDeleted).ToList()
                });

            var postsResult = await posts.ToListAsync();

            return new PagedResult<Post>(postsResult, page, pageSize, count);
        }

        public async Task<Post> GetAsync(int id)
        {
            var post = await db
                .Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);
            return post;
        }

        public async Task<List<Post>> FindAsync(Expression<Func<Post, bool>> predicate)
        {
            var posts = await db
                .Posts
                .Where(predicate)
                .Include(p => p.PostFiles)
                .ThenInclude(f => f.File)
                .ToListAsync();
            return posts;
        }

        public async Task<Post> CreateAsync(Post post)
        {
            var createdPost = await db.Posts.AddAsync(post);

            return createdPost.Entity;
        }

        public async Task<Post> Update(Post post)
        {
            db.Entry(post).State = EntityState.Modified;
            return post;
        }


        public async Task<List<Post>> FindValidAsync(Expression<Func<Post, bool>> predicate)
        {
            var posts = await db
                .Posts
                .Where(predicate)
                .Where(u => !u.IsDeleted)
                .Include(p => p.PostFiles)
                .ThenInclude(f => f.File)
                .ToListAsync();
            return posts;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await db.Posts.FindAsync(id);
            if (post == null)
                return false;
            post.IsDeleted = true;
            return true;
        }
    }
}