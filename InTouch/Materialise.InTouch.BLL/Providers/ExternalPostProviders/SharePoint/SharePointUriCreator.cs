using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint
{
    public static class SharePointUriCreator
    {
        public static string GetListFeedUri(string listTitle)
        {
            return $"lists/getbytitle('{listTitle}')/" +
                   "items?" +
                   "$select=Title,Body,Created,Attachments,Id" +
                   "&$orderby=Created desc" +
                   "&$top=15";
        }
        public static string GetImageFeedUri(string listTitle,int id)
        {
            return $"lists/getbytitle('{listTitle}')/" +
                $"items({id.ToString()})?" +
                $"$select=Attachments,AttachmentFiles" +
                $"&$expand=AttachmentFiles";
        }
    }   
}
