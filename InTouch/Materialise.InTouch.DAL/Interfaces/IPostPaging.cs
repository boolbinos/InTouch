using Materialise.InTouch.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Materialise.InTouch.DAL.Interfaces
{
    public interface IPostPaging<T> where T: class
    {
        Task<PagedResult<T>> GetPageAsync(int id, int page, int pageSize, bool justMyPosts, string srchStr = null);
    }
}
