using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Authorize.Avatar
{
    public interface IAvatarClient
    {
        Task<byte[]> GetAvatar(string userProfile);
    }
}
