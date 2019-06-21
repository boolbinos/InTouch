using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Materialise.InTouch.DAL.Infrastructure;

namespace Materialise.InTouch.DAL.Repositories
{
    public class FileRepository : IRepository<FileInfo, Guid>
    {
        private readonly InTouchContext _db;

        public FileRepository(InTouchContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<FileInfo> CreateAsync(FileInfo file)
        {
            var createdFile = await _db.Files.AddAsync(file);

            return createdFile.Entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var file = await _db
                .Files
                .FirstAsync(f => f.Id == id && f.IsDeleted == false);

            if (file == null)
                return false;

            file.IsDeleted = true;

            return true;
        }

        public Task<List<FileInfo>> FindAsync(Expression<Func<FileInfo, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FileInfo>> FindValidAsync(Expression<Func<FileInfo, bool>> predicate)
        {
            var files = await _db
                .Files
                .Include(f => f.PostFile)
                .Where(predicate)
                .Where(f => f.IsDeleted == false)
                .ToListAsync();

            return files;
        }

        public async Task<List<FileInfo>> GetAllValidAsync()
        {
            var files = await _db
                .Files
                .ToListAsync();

            return files;
        }

        public async Task<List<FileInfo>> GetAllAsync()
        {
            var files = await _db
                .Files
                .Where(f => f.IsDeleted == false)
                .ToListAsync();

            return files;
        }

        public async Task<FileInfo> GetAsync(Guid id)
        {
            var file = await _db
                .Files
                .Include(f=>f.PostFile)
                .FirstOrDefaultAsync(f => f.Id == id && f.IsDeleted == false);

            return file;
        }

        public async Task<FileInfo> Update(FileInfo file)
        {
            _db.Entry(file).State = EntityState.Modified;

            return file;
        }
    }
}