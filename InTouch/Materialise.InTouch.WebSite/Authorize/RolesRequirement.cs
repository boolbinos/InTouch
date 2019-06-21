using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Authorize
{
    public class RolesRequirement : IAuthorizationRequirement
    {
        public string[] Roles { get; private set; }

        public RolesRequirement(params string[] Roles)
        {
            this.Roles = Roles;
        }
    }
}
