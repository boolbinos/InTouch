using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Materialise.InTouch.BLL.Interfaces;
using System.Security.Claims;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.WebSite;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Net.Http;
using Materialise.InTouch.BLL.Services.Exceptions;
using Microsoft.Extensions.Options;

namespace Materialise_InTouch_WebSite.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult KeepSessionAlive()
        {
            return Ok();
        }
        public IActionResult Error(string id)
        {
            int statusCode = 500;
            string message = "Error";

            if (id == null)  //Exception
            {
                var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
                if (exception != null)
                {
                    message = exception.Error.Message;

                    var type = exception.Error.GetType();
                    statusCode = type == typeof(NotFoundException) ?
                        404 : HttpContext.Response.StatusCode;
                }
            }
            else  //StatusCode
            {
                Int32.TryParse(id, out statusCode);
                var response = new HttpResponseMessage((HttpStatusCode)statusCode);
                message = response.ReasonPhrase;
            }

            return StatusCode(statusCode, message);
        }

        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignedOut), "Home", values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }
        public void AccessDenied()
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
    }
}
