using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models
{
    public class SharepointPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public bool Attachment { get; set; }
        public List<SharepointPostImages> ImagesInBodyPost { get; set; } // = new List<SharepointPostImages>();
    }
    public class SharepointPostImages
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public Stream file { get; set; }
    }
}
