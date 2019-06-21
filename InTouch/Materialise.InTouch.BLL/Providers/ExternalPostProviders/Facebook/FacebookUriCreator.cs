namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook
{
    public static class FacebookUriCreator
    {
        public static string GetPageFeedUri(string pageId)
        {
            return pageId + "/posts";
        }

        public static string GetPostAttachmentsUri(string postId)
        {
            return postId + "/attachments";
        }
    }
}