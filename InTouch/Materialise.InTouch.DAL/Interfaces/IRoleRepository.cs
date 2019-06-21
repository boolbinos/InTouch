using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Materialise.InTouch.DAL.Interfaces
{
    public interface IRoleRepository<T, TId> : IRepository<T,TId> where T : class
    {
        Task<T> GetByNameAsync(string roleName);
    }
}
