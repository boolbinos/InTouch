using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models.Config
{
    public class SharePointPostProviderConfig
    {
        public List<SharePointList> Lists { get; set; }
        public Credential Credential { get; set; }
    }

    public class SharePointList
    {
        public string Title { get; set; }
        public string BasePathExtension { get; set; }
    }
    public class Credential
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
    }
}
