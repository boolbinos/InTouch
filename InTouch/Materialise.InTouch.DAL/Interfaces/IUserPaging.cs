using Materialise.InTouch.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Materialise.InTouch.DAL.Interfaces
{
    public interface IUserPaging<T> where T:class
    {
        Task<PagedResult<T>> GetPageAsync(int page, int pageSize);
    }
}
