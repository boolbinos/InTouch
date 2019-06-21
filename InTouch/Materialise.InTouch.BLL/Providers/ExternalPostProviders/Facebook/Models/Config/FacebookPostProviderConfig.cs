using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models.Config
{
    public class FacebookPostProviderConfig
    {
        public FacebookApplication FacebookApplication { get; set; }
        public List<FacebookPage> Pages { get; set; }
        public string DefaultTitle { get; set; }
    }

    public class FacebookPage
    {
        public string PageId { get; set; }
    }
    public class FacebookApplication
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
