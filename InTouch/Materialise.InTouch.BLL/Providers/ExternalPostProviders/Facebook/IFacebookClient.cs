using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook
{
    public interface IFacebookClient
    {
        Task<IList<AttachmentData>> GetPostAttachments(string postId);
        Task<IList<FacebookPost>> GetPosts(string pageId);
        Task<string> GetPageName(string pageId);
        Task<HttpResponseMessage> GetImage(string url);
    }
}