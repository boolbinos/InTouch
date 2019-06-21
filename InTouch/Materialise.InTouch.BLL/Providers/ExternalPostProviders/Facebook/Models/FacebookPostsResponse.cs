using System.Collections.Generic;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models
{
    public class FacebookPost
    {
        public string Story { get; set; }
        public string Id { get; set; }
        public string Created_time { get; set; }
        public string Message { get; set; }
    }

    public class FacebookPostsResponse
    {
        public FacebookPostsResponse()
        {
            Data = new List<FacebookPost>();
        }
        public List<FacebookPost> Data { get; set; }
    }
}