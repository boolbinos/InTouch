using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Materialise.InTouch.DAL.Infrastructure;

namespace Materialise.InTouch.DAL.Interfaces
{
    public interface IRepository<T,TId> where T : class
    {
        Task<List<T>> GetAllValidAsync();
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(TId id);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> FindValidAsync(Expression<Func<T, bool>> predicate);
        Task<T> CreateAsync(T item);
        Task<T> Update(T item);
        Task<bool> DeleteAsync(TId id);
    }
}
