using AlphaPlus.Features.Handbook.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AlphaPlus.Core.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace AlphaPlus.Features.Site.Handbook.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class HandbookFilterAttribute : TypeFilterAttribute
{
    public HandbookFilterAttribute() : base(typeof(HandbookFilter)) { }

    public class HandbookFilter() : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated || 
                context.HttpContext.Session.Get<int>("CUSTID") == 0)
            {
                await UnAuthorization(context);
                return;
            }

            if (AppSettingsHelper.SupportedNewHandbook)
            {
                return;
            }

            await UnAuthorization(context);
        }

        private static async Task UnAuthorization(AuthorizationFilterContext context)
        {
            if (IsAjaxRequest(context.HttpContext.Request))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "You are not authorized on this function!" });
            }
            else
            {
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        private static bool IsAjaxRequest(HttpRequest request)
            => request.Headers != null && request.Headers.XRequestedWith == "XMLHttpRequest";
    }
}