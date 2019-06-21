using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.WebSite.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Services.EmailService
{
    public interface IEmailNotificationSender
    {
        Task PostCreatedNotificationAsync(PostViewModel post);
        Task PostCommentNotificationAsync(CommentViewModel comment, PostDTO post, List<CommentEmailModel> commentators);
    }
}
