using System.Collections.Generic;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.BLL.Interfaces
{
    public interface IExternalPostService
    {
        Task<int> ImportPostsAsync(string providerName);
    }
}