using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models.Config;
using Materialise.InTouch.WebSite.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Authorize.Avatar
{
    public class AvatarClient : IAvatarClient
    {
        private readonly HttpClient _avatarHttpClient;
        private readonly AzureAdOptions _credential;
        public AvatarClient(IOptionsSnapshot<AzureAdOptions> options)
        {
            _credential = options.Value;
            var servicePoint = new Uri("https://graph.window.net");
            var serviceRoot = new Uri(servicePoint, _credential.Domain);
            var authContext = new AuthenticationContext($"htts://login.windows.net/{_credential.Domain}/oauth2/token");
            var credential = new ClientCredential(_credential.ClientId, _credential.ClientSecret);
            var _avatarHttpClient = new HttpClient();
            var getToken =  authContext.AcquireTokenAsync(servicePoint.OriginalString, credential).Result;
            _avatarHttpClient.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", getToken.AccessToken);
        }
        public async Task<byte[]> GetAvatar(string userProfile)
        {
            var uri = $"https://graph.window.net/{_credential.Domain}/users/{userProfile}/thumbnailPhoto?api-version=1.6";

            var response = await _avatarHttpClient.GetAsync(new Uri(uri));

            if(response.IsSuccessStatusCode)
            {
                var photo = await response.Content.ReadAsByteArrayAsync();
                return photo;
            }
            return null;
        }
    }
}
