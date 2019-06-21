using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models;
using Materialise.InTouch.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders
{
    public interface IFacebookPostCreator
    {
        Task<string> GetPageName(string pageId);
        Task<IList<AttachmentData>> GetPostAttachments(string postId);
        Task<IList<FacebookPost>> GetRawPosts(string pageId);
        Task<ICollection<PostFile>> GetPostImages(AttachmentData attachments, string postId);
    }
}
