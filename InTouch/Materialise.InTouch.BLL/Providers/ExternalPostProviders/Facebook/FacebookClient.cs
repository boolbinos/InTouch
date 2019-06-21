using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook
{
    public class FacebookClient : IFacebookClient
    {
        private readonly HttpClient _facebookHttpClient;
        private readonly string _appId;
        private readonly string _appSecret;

        public FacebookClient(IOptionsSnapshot<FacebookPostProviderConfig> options)
        {
            _facebookHttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v2.8/")
            };

            _facebookHttpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _appId = options.Value.FacebookApplication.AppId;
            _appSecret = options.Value.FacebookApplication.AppSecret;
        }

        public async Task<IList<AttachmentData>> GetPostAttachments(string postId)
        {
            var response = await RequestAsync<AttachmentResponse>(FacebookUriCreator.GetPostAttachmentsUri(postId));
            return response.Data;
        }

        public async Task<IList<FacebookPost>> GetPosts(string pageId)
        {
            var response = await RequestAsync<FacebookPostsResponse>(FacebookUriCreator.GetPageFeedUri(pageId));
            return response.Data;
        }

        public async Task<string> GetPageName(string pageId)
        {
            var response = await RequestAsync<PageNameResponse>(pageId);
            return response.Name;
        }

        public async Task<HttpResponseMessage> GetImage(string url)
        {
            var response = await _facebookHttpClient.GetAsync(new Uri(url));
            response.EnsureSuccessStatusCode();
            return response;
        }

        private async Task<T> RequestAsync<T>(string path)
        {
            var uri = "https://graph.facebook.com/" + path + $"?access_token={_appId}|{_appSecret}";
            var response = await _facebookHttpClient.GetAsync(new Uri(uri));

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}