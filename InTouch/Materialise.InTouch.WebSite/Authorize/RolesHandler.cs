using Materialise.InTouch.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Authorize
{
    public class RolesHandler : AuthorizationHandler<RolesRequirement>
    {
        private IUserService _userConfig;
        public RolesHandler(IUserService userConfig)
        {
            _userConfig = userConfig;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesRequirement requirement)
        {
            var nameClaim = context.User.FindFirst(ClaimTypes.Name);
            var redirectContext = context.Resource as AuthorizationFilterContext;
            if (nameClaim != null)
            {
                var getCurrentUser = await _userConfig.GetByEmailAsync(nameClaim.Value);
                var CheckRole = requirement.Roles.Contains(getCurrentUser.RoleDTO.Name);
                if (!CheckRole)
                {
                    redirectContext.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                    context.Succeed(requirement);
                    await Task.CompletedTask;
                }
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
            else
            {
                redirectContext.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
        }
    }
}
