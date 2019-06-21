using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Materialise.InTouch.DAL.Interfaces;
using Materialise.InTouch.DAL.Infrastructure;
using System;
using System.Linq.Expressions;

namespace Materialise.InTouch.DAL.Repositories
{
    public class RoleRepository : IRoleRepository<Role, int>
    {
        private InTouchContext db;

        public RoleRepository(InTouchContext context)
        {
            this.db = context;
        }

        public async Task<List<Role>> GetAllValidAsync()
        {
            var roles = await db.Roles.ToListAsync();
            return roles;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            throw new NotImplementedException();
        }


        public async Task<Role> GetAsync(int id)
        {
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == id);
            return role;
        }

        public Task<List<Role>> FindAsync(Expression<Func<Role, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> GetByNameAsync(string roleName)
        {
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            return role;
        }

        public Task<Role> CreateAsync(Role item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Role>> FindValidAsync(Expression<Func<Role, bool>> predicate)
        {
            throw new NotImplementedException();
        }


        public Task<Role> Update(Role item)
        {
            throw new NotImplementedException();
        }
    }
}