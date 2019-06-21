using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.WebSite.Extensions;
using Materialise.InTouch.WebSite.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Services.EmailService
{
    public class EmailNotificationService : IEmailNotificationSender
    {
        private readonly IUserService _userService;
        private readonly EmailSettings _emailSettings;
        private readonly UrlsInfo _urlsInfo;
        private readonly ICommentService _commentService;
        public ILogger<EmailNotificationService> _logger;
        private readonly IPostService _postService;
        private readonly string _templatePath;

        public EmailNotificationService(IUserService userService, IOptionsSnapshot<EmailSettings> emailSettings,
            IOptionsSnapshot<UrlsInfo> urlsInfo, ILogger<EmailNotificationService> logger, ICommentService commentService,
            IPostService postService, IHostingEnvironment env)
        {
            _emailSettings = emailSettings.Value;
            _userService = userService;
            _urlsInfo = urlsInfo.Value;
            _logger = logger;
            _commentService = commentService;
            _postService = postService;
            _templatePath = Path.Combine(env.WebRootPath, "EmailTemplates\\Email.html");
        }

        public async Task PostCreatedNotificationAsync(PostViewModel post)
        {
            var moderators = (await _userService.FindValidAsync(user =>
            user.IsModerator()
            && user.FirstName != "System"
            && user.IsDeleted == false
           ))
            .Select(m => new { m.Email, m.FirstName, m.LastName })
            .ToList();

            string subject = "InTouch | New post notification";
            var postUrl = _urlsInfo.BaseUrl + _urlsInfo.PostDetailUrl + post.Id;
            var dateYear = DateTime.Now.Year;

            var html = File.ReadAllText(_templatePath);

            moderators.ForEach(async m =>
            {
                var text = $"New post:  <a href=\"{_urlsInfo.BaseUrl}{_urlsInfo.PostDetailUrl}{post.Id}\">{post.Title}</a> was added by {post.UserName} at {post.CreatedDate}";
                var body = string.Format(html, m.FirstName, m.LastName, text, " ", postUrl, dateYear);
                await ExecuteAsync(new List<string> { m.Email }, subject, body);
            });
        }
        public async Task PostCommentNotificationAsync(CommentViewModel comment, PostDTO post, List<CommentEmailModel> commentators)
        {
            var html = File.ReadAllText(_templatePath);

            var subject = "InTouch | Comments notification";
            var postUrl = _urlsInfo.BaseUrl + _urlsInfo.PostDetailUrl + post.Id;
            var dateYear = DateTime.Now.Year;
            var commentContent = $"\"{comment.Content}\"";

            if (post.UserId != comment.UserId && post.UserDTO.FirstName != "System")
            {
                var text = $"{comment.UserFirstName} {comment.UserLastName} has left comment under your post:  <a href=\"{_urlsInfo.BaseUrl}{_urlsInfo.PostDetailUrl}{post.Id}\">{post.Title}</a>";
                var body = string.Format(html, post.UserDTO.FirstName, post.UserDTO.LastName, text, commentContent, postUrl, dateYear);
                await ExecuteAsync(new List<string>() { post.UserDTO.Email }, subject, body);
            }

            commentators.ForEach(async c =>
            {
                var text = $"{comment.UserFirstName} {comment.UserLastName} has also commented on the post <a href=\"{_urlsInfo.BaseUrl}{_urlsInfo.PostDetailUrl}{post.Id}\">{post.Title}</a>";
                var body = string.Format(html, c.FirstName, c.LastName, text, commentContent, postUrl, dateYear);
                await ExecuteAsync(new List<string> { c.Email }, subject, body);
            });

        }
        private async Task ExecuteAsync(List<string> toCollection, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                From = new MailAddress(_emailSettings.FromEmail)
            };

            toCollection.ForEach(email => mailMessage.To.Add(email));

            using (var smtpClient = new SmtpClient(_emailSettings.HostName, _emailSettings.Port))
            {
                try
                {
                    smtpClient.UseDefaultCredentials = true;
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (SmtpException ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }
    }
}
