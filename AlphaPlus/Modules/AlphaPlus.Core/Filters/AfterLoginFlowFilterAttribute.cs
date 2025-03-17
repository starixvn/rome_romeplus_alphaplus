using AlphaPlus.Core.Models;
using AlphaPlus.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Core.Filters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class AfterLoginFlowFilterAttribute(IUserSession userSession, INicknameChecker nicknameChecker) : ActionFilterAttribute
{
    private readonly IUserSession userSession = userSession;
    private readonly INicknameChecker nicknameChecker = nicknameChecker;

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        base.OnActionExecuting(filterContext);

        if (IsValidSession(filterContext))
        {
            var redirectUrl = GetAfterLoginFlowUrl(filterContext);

            if (redirectUrl != null)
            {
                filterContext.Result = new RedirectResult(redirectUrl);
            }
        }
    }

    private bool IsValidSession(ActionExecutingContext filterContext)
        => filterContext.HttpContext?.Session != null
        && userSession.UserId > 0
        && !userSession.IsInternal;

    private string GetAfterLoginFlowUrl(ActionExecutingContext filterContext)
    {
        var controllerName = filterContext.RouteData.Values["controller"].ToString();
        var actionName = filterContext.RouteData.Values["action"].ToString();
        var currentUrl = $"{controllerName}/{actionName}";

        if (!IsInIgnoreUrl(currentUrl))
        {
            if (IsForceChangePassword)
            {
                return ForceChangePasswordUrl(currentUrl);
            }

            if (userSession.IsForcedSecurityCode)
            {
                return ForceChangeSecurityCodeUrl(currentUrl, userSession.SecurityCode);
            }

            if (IsForceLoginByNickname)
            {
                return CreateNickNameUrl(currentUrl);
            }
        }

        return null;
    }

    private static bool IsInIgnoreUrl(string currentUrl)
        => currentUrl.Equals("Language/Change", StringComparison.OrdinalIgnoreCase)
        || currentUrl.Equals("Default/Kickout", StringComparison.OrdinalIgnoreCase)
        || currentUrl.Equals("Account/Logout", StringComparison.OrdinalIgnoreCase)
        || currentUrl.Equals("Account/Logout2", StringComparison.OrdinalIgnoreCase);

    private bool IsForceChangePassword
        => !userSession.IsLogged || userSession.IsPasswordExpired;

    private static string ForceChangePasswordUrl(string currentUrl)
        => currentUrl.Contains("ForceChangePassword", StringComparison.OrdinalIgnoreCase) ||
        currentUrl.Contains("SkipForceChangePassword", StringComparison.OrdinalIgnoreCase)
        ? null
        : MicroSites.MainUrl("~/Password/ForceChangePassword");

    private static string ForceChangeSecurityCodeUrl(string currentUrl, string securityCode)
    {
        if (string.IsNullOrWhiteSpace(securityCode))
        {
            return currentUrl.Equals("SecurityCode/ChangeSecurityCodeFirstLogin", StringComparison.OrdinalIgnoreCase)
                ? null
                : MicroSites.MainUrl("~/SecurityCode/ChangeSecurityCodeFirstLogin");
        }

        return currentUrl.Equals("SecurityCode/ChangeSecurityCodeByForcing", StringComparison.OrdinalIgnoreCase)
            ? null
            : MicroSites.MainUrl("~/SecurityCode/ChangeSecurityCodeByForcing");
    }

    private bool IsForceLoginByNickname => nicknameChecker.IsForceLoginByNickname;

    private static string CreateNickNameUrl(string currentUrl)
        => currentUrl.Equals("Nickname/Index", StringComparison.OrdinalIgnoreCase)
        ? null
        : MicroSites.MainUrl("~/Nickname/Index");

    private static bool IsAjaxRequest(HttpRequest request)
        => request.Headers != null && (request.Headers.XRequestedWith == "XMLHttpRequest" || request.Method == "POST");
}
