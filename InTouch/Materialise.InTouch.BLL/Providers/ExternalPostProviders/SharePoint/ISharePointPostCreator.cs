using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models.Config;
using Materialise.InTouch.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint
{
    public interface ISharePointPostCreator
    {
        Task<IList<SharepointPost>> GetPosts(SharePointList list);

        Task<IList<SharepointPostImages>> GetImages(SharePointList list, int id);

        Task<ICollection<PostFile>> GetPostImages(IList<SharepointPostImages> attachments, int postId);
    }
}
