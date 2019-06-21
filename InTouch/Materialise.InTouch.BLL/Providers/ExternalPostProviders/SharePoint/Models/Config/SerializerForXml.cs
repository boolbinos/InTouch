using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models.Config
{
    public static class SerializerForXml
    {
        private static XNamespace xnameAtom = "http://www.w3.org/2005/Atom";
        private static XNamespace xnameMicrosoft = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        private static XNamespace xnameMicrosoft2 = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

        public static async  Task<IList<SharepointPost>> DeserializeToSharePointPost(string xmlstring)
        {
            xmlstring = xmlstring.Replace("d:ID", "d:Id");
            XDocument xdoc = XDocument.Parse(xmlstring);
            
            var posts = xdoc.Element(xnameAtom + "feed").Elements(xnameAtom + "entry").Select(i => {
                var properties = i.Element(xnameAtom + "content").Element(xnameMicrosoft2 + "properties");
                return new SharepointPost
                {
                    Id = Convert.ToInt32(properties.Element(xnameMicrosoft + "Id").Value),
                    Title = properties.Element(xnameMicrosoft + "Title").Value,
                    Body = properties.Element(xnameMicrosoft + "Body").Value,
                    Created = Convert.ToDateTime(properties.Element(xnameMicrosoft + "Created").Value),
                    Attachment = Convert.ToBoolean(properties.Element(xnameMicrosoft + "Attachments").Value)
                };
            }).ToList();

            return posts;
        }
        public static async Task<IList<SharepointPostImages>> DeserializeToSharePointPostBody(string xmlstring,Func<string, string, Task<SharepointPostImages>> fileWriter)
        {
            var files = new List<SharepointPostImages>();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xmlstring);
            
            var elemRef = doc.GetElementsByTagName("d:ServerRelativeUrl");

            var elemName = doc.GetElementsByTagName("d:FileName");

            for (int i=0;i< elemName.Count;i++)
            {
                var fileUpload = await fileWriter(elemRef[i].InnerText, elemName[i].InnerText);
                if (fileUpload!=null)
                {
                    files.Add(fileUpload);
                }
            }
            return files;
        }
        public static async Task DeserializeToSharePointPostImagesInBody(IList<SharepointPost> posts, Func<string, string, Task<SharepointPostImages>> fileWriter, string basePath)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            foreach (var post in posts)
            {
                htmlDoc.LoadHtml(post.Body);

                await GetImagesFromBody(fileWriter, htmlDoc, post);
                FixLinks(htmlDoc, post, basePath);
            }
        }

        private static void FixLinks(HtmlDocument htmlDoc, SharepointPost post, string basePath)
        {
            var links = htmlDoc.DocumentNode.SelectNodes("//a");

            if (links != null)
            {
                foreach (var link in links)
                {
                    var href = link.GetAttributeValue("href", string.Empty);

                    if (href != string.Empty && href.StartsWith('/'))
                        link.SetAttributeValue("href", String.Concat(basePath, href));
                }
            }

            post.Body = htmlDoc.DocumentNode.InnerHtml;
        }

        private static async Task GetImagesFromBody(Func<string, string, Task<SharepointPostImages>> fileWriter, HtmlDocument htmlDoc, SharepointPost post)
        {
            var images = htmlDoc.DocumentNode.SelectNodes("//img");

            post.ImagesInBodyPost = new List<SharepointPostImages>();

            if (images != null)
            {
                var getImagesinBody = images.AsEnumerable().Select(d => new
                {
                    src = d.Attributes["src"].Value,
                    alt = d.Attributes["alt"] == null
                    ? post.Id.ToString() + "_SharepoinImages" : String.IsNullOrEmpty(d.Attributes["alt"].Value)
                    ? post.Id.ToString() + "_SharepoinImages" : d.Attributes["alt"].Value
                });


                foreach (var img in getImagesinBody)
                {
                    var fileUpload = await fileWriter(img.src, img.alt);
                    if (fileUpload != null)
                    {
                        post.ImagesInBodyPost.Add(fileUpload);
                    }
                }
                htmlDoc.DocumentNode.Descendants().Where(q => q.Name == "img").ToList().ForEach(w => w.Remove());
                post.Body = htmlDoc.DocumentNode.InnerHtml;
            }
        }
    }
}
