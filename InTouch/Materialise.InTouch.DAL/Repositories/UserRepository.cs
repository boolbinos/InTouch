using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Materialise.InTouch.DAL.Infrastructure;

namespace Materialise.InTouch.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private InTouchContext db;

        public UserRepository(InTouchContext context)
        {
            this.db = context;
        }

        public async Task<List<User>> GetAllValidAsync()
        {
           return await db.Users
                .Where(u => !u.IsDeleted)
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await db.Users
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<PagedResult<User>> GetPageAsync(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;

            var count = db.Users.Count();

            var posts = await db.Users
                .Where(p => !p.IsDeleted)
                .Include(u => u.Role)
                .OrderByDescending(p => p.LastName)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            return new PagedResult<User>(posts, page, pageSize, count);
        }

        public async Task<User> GetAsync(int id)
        {
            return await db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);
        }

        public async Task<List<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<User> CreateAsync(User user)
        {
            var createdUser = await db.Users.AddAsync(user);
            return createdUser.Entity;
        }

        public async Task<User> Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            return user;
        }

        public async Task<List<User>> FindValidAsync(Expression<Func<User, bool>> predicate)
        {
            return await db.Users.Where(predicate).Include(u=>u.Role).ToListAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
                return false;
            user.IsDeleted = true;
            return true;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            email = email.ToUpper();
            var user = await db.Users
                .Include(u => u.Role).AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToUpper() == email);

            return user;
        }

    }
}
