using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models
{
    public class Image
    {
        public int Height { get; set; }
        public string Src { get; set; }
        public int Width { get; set; }
    }

    public class Media
    {
        public Image Image { get; set; }
    }

    public class Target
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }

    public class SubAttachmentData
    {
        public Media Media { get; set; }
        public Target Target { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }

    public class SubAttachments
    {
        public IList<SubAttachmentData> Data { get; set; }
    }

    public class AttachmentData
    {
        public string Description { get; set; }
        public Media Media { get; set; }
        public SubAttachments Subattachments { get; set; }
        public Target Target { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
    public class AttachmentResponse
    {
        public AttachmentResponse()
        {
            Data = new List<AttachmentData>();
        }
        public IList<AttachmentData> Data { get; set; }
    }
}
