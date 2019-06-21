using System;
//using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Xml.Serialization;
using System.IO;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models.Config;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using HtmlAgilityPack;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint
{
    public class SharePointClient : ISharePointClient
    {
        private readonly HttpClient _sharepointHttpClient;
        private readonly Credential _credential;
        public SharePointClient(IOptionsSnapshot<SharePointPostProviderConfig> options)
        {
            _credential = options.Value.Credential;
            var handler = new HttpClientHandler();
            KeepCredential(ref handler);

            _sharepointHttpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://home.materialise.net")
            };
            _sharepointHttpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/atom+xml"));            
        }

        public async Task<IList<SharepointPost>> GetListPostAsync(string pageName, string basePathExtension = "")
        {
            var response = await RequestAsync(SharePointUriCreator.GetListFeedUri(pageName), basePathExtension);
            var getpost = await SerializerForXml.DeserializeToSharePointPost(response);

            await SerializerForXml.DeserializeToSharePointPostImagesInBody(getpost, GetImagesByte, _sharepointHttpClient.BaseAddress.ToString());
            
            return getpost;
        }
        public async Task<IList<SharepointPostImages>> GetImages(string pageName, int id, string basePathExtension = "")
        {
            var response = await RequestAsync(SharePointUriCreator.GetImageFeedUri(pageName,id),basePathExtension);
            var Result = await SerializerForXml.DeserializeToSharePointPostBody(response, GetImagesByte);
            return Result;
        }
        
        private async Task<SharepointPostImages> GetImagesByte(string xmlFile,string xmlFileName)
        {
            var uri = $"{_sharepointHttpClient.BaseAddress}{xmlFile}";

            var response = await _sharepointHttpClient.GetAsync(new Uri(uri));

            //response.EnsureSuccessStatusCode();

            var files = await response.Content.ReadAsStreamAsync();

            if (files.Length < 2000)
            {
                return null;
            }

            return new SharepointPostImages
            {
               Name = xmlFileName,
               ContentType = response.Content.Headers.ContentType.MediaType,
               file = files
            };
        }
                        
        private async Task<string> RequestAsync(string path, string basePathExtension = "")
        {
            var uri = $"{_sharepointHttpClient.BaseAddress}/{basePathExtension}_api/{path}";
            var response = await _sharepointHttpClient.GetAsync(new Uri(uri));

            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync();
            
            return xml;
        }
        
        private  void KeepCredential(ref HttpClientHandler handler)
        {
            if (String.IsNullOrEmpty(_credential.Name) || String.IsNullOrEmpty(_credential.Password) || String.IsNullOrEmpty(_credential.Domain))
            {
                handler.UseDefaultCredentials = true;
            }
            else
            {
                handler.Credentials = new NetworkCredential(_credential.Name, _credential.Password, _credential.Domain);
            }
        }
    }
}
