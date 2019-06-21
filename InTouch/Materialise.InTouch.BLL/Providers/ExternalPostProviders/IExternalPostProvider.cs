using Materialise.InTouch.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders
{
    public interface IExternalPostProvider
    {
        Task<List<Post>> GetPostsAsync(DateTime? lastSyncDate);
    }
}