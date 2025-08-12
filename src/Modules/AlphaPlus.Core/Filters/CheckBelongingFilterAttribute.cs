using System.Text.Json;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Constants;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Core.Filters;

public class CheckBelongingFilterAttribute : TypeFilterAttribute
{
    public CheckBelongingFilterAttribute() : base(typeof(CheckBelongingFilter))
    {
    }

    private class CheckBelongingFilter(IOptions<AuthorizationOptions> options, IAuthorizationService authorizationService) : IAsyncAuthorizationFilter
    {
        private readonly AuthorizationOptions options = options.Value;
        private readonly IAuthorizationService authorizationService = authorizationService;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated ||
                context.HttpContext.Session.Get<int>("CUSTID") == 0)
            {
                await Fanex.Logging.LogService.InfoAsync("Mat session");
                await UnAuthorization(context);
                return;
            }

            if (UserSession.Current.IsInternal)
            {
                return;
            }

            if (options.EnableCheckBelonging)
            {
                if (!await authorizationService.IsBelonging())
                {
                    await Fanex.Logging.LogService.InfoAsync("IsBelonging = false");
                    await UnAuthorization(context);
                    return;
                }

                if (!authorizationService.IsAuthorized())
                {
                    await KickOutUser(context, "NoPermission");
                    return;
                }
            }

            var accountStatus = await authorizationService.GetAccountStatus();

            if (accountStatus != null && (accountStatus.IsClosed || accountStatus.IsLocked))
            {
                await KickOutUser(context, accountStatus.IsClosed ? "AccountClosed" : "AccountLocked");
                return;
            }

            if (!options.DisabledCheckDuplicationLogin)
            {
                var kickOutInfo = await authorizationService.IsKickedOut();

                if (kickOutInfo.IsKickedOut)
                {
                    await KickOutUser(context, GetKickOutReasonMessage(kickOutInfo.KickOutReason));
                }
            }
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
                var controllerName = context.RouteData.Values["controller"].ToString();
                var actionName = context.RouteData.Values["action"].ToString();
                var currentUrl = $"{controllerName}/{actionName}";

                if (!currentUrl.Equals("Account/Login", StringComparison.OrdinalIgnoreCase))
                {
                    context.Result = new RedirectResult(MicroSites.MainUrl("~/Account/Login?r=You are not authorized on this function!"));
                }
            }
        }

        private Task KickOutUser(AuthorizationFilterContext context, string message)
        {
            var kickOutPageAbsoluteUrl = MicroSites.MainUrl($"{options.KickOutUrl}");
            var currentPageAbsoluteUrl = context.HttpContext.Request.GetDisplayUrl();
            if (currentPageAbsoluteUrl.Contains(kickOutPageAbsoluteUrl, StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            if (IsAjaxRequest(context.HttpContext.Request))
            {
                context.Result = new UnauthorizedObjectResult(JsonSerializer.Serialize(new { message }));
                return Task.CompletedTask;
            }

            context.Result = new RedirectResult($"{kickOutPageAbsoluteUrl}?reason={message}");
            return Task.CompletedTask;
        }

        private static string GetKickOutReasonMessage(byte kickOutReason)
            => kickOutReason switch
            {
                KickOutReasons.ResetPasswordForceKickOut => "KickoutByForceChangePassword",
                KickOutReasons.ProtectedModeForceKickOut => "KickoutByProtectedMode",
                _ => "DuplicationLogin",
            };

        private static bool IsAjaxRequest(HttpRequest request)
            => request.Headers != null && (request.Headers.XRequestedWith == "XMLHttpRequest" || request.Method == "POST");
    }
}
