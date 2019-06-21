using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.WebSite.Authorize.Avatar;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Authorize
{
    public class CheckUserAttribute : IAsyncResourceFilter
    {
        private readonly IUserService _userInfo;
       // private readonly IAvatarClient _avatarClient;
        public CheckUserAttribute(IUserService userInfo)
        {
            _userInfo = userInfo;
           // _avatarClient = avatarClient;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {

            var nameClaim = context.HttpContext.User.FindFirst(ClaimTypes.Name);

            if (nameClaim == null)
            {
                throw new InvalidOperationException();
            }

            var getCurrentUser = await _userInfo.GetByEmailAsync(nameClaim.Value);
            if (getCurrentUser == null)
            {
                //  var getAvatar = await _avatarClient.GetAvatar(context.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
                var user = new CreateUserDTO()
                {
                    Email = nameClaim.Value,
                    FirstName = context.HttpContext.User.FindFirst("name").Value.Split(" ")[0],
                    LastName = context.HttpContext.User.FindFirst("name").Value.Split(" ")[1]
                    //Avatar= getAvatar
                };

                getCurrentUser = await _userInfo.CreateAsync(user);
            }
            //if(getCurrentUser.Avatar==null && getCurrentUser != null)
            //{
            //    var getAvatar = await _avatarClient.GetAvatar(context.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            //    getCurrentUser.Avatar = getAvatar;
            //    await _userInfo.Update(getCurrentUser, getCurrentUser.Id);
            //}

            if (context.HttpContext.Items.ContainsKey(Consts.HttpItemNames.CurrentUser))
            {
                context.HttpContext.Items[Consts.HttpItemNames.CurrentUser] = getCurrentUser;
            }
            else
            {
                context.HttpContext.Items.Add(Consts.HttpItemNames.CurrentUser, getCurrentUser);
            }
            await next.Invoke();
        }
    }
}
