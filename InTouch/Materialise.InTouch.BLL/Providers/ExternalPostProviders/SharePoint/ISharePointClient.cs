using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint
{
    public interface ISharePointClient
    {
        Task<IList<SharepointPost>> GetListPostAsync(string pageName, string basePathExtension = "");
        Task<IList<SharepointPostImages>> GetImages(string pageName, int id, string basePathExtension = "");
    }
}
