using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.ModelsDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite
{
    public class WebSiteUserContext : IUserContext
    {
        private IHttpContextAccessor _httpContextAccessor;

        public WebSiteUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserDTO CurrentUser
        {
            get
            {
                if (!_httpContextAccessor.HttpContext.Items.ContainsKey(Consts.HttpItemNames.CurrentUser))
                {
                    throw new InvalidOperationException("user does not exist.");
                }

                return (UserDTO)_httpContextAccessor.HttpContext.Items[Consts.HttpItemNames.CurrentUser];
            }

        }
    }
}
