using System;
using System.Web.Mvc;
using AlphaPlus.User.Models;
using Alpha.Web.Helpers;

namespace Alpha.Web.Filters
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AfterLoginFlowFilter : ActionFilterAttribute
    {
        private bool isAuthorizedChecking;

        public AfterLoginFlowFilter(bool isAuthorizedChecking = true)
        {
            this.isAuthorizedChecking = isAuthorizedChecking;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated
                   && !isAuthorizedChecking)
            {
                isAuthorizedChecking = true;
            }

            if (!isAuthorizedChecking)
            {
                return;
            }

            if (filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath.EndsWith("/Auth"))
            {
                return;
            }

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
            && !filterContext.IsChildAction
            && UserSession.Current.UserId > 0
            && !UserSession.Current.IsInternal;

        private string GetAfterLoginFlowUrl(ActionExecutingContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;
            var controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            var currentUrl = $"{controllerName}/{actionDescriptor.ActionName}";

            if (!IsInIgnoreUrl(currentUrl))
            {
                if (IsForceChangePassword)
                {
                    return ForceChangePasswordUrl(currentUrl);
                }

                if (UserSession.Current.IsForcedSecurityCode)
                {
                    return ForceChangeSecurityCodeUrl(currentUrl, UserSession.Current.SecurityCode);
                }

                if (IsForceLoginByNickname)
                {
                    return CreateNickNameUrl(currentUrl);
                }
            }

            return null;
        }

        private bool IsInIgnoreUrl(string currentUrl)
            => currentUrl.Equals("Language/Change", StringComparison.OrdinalIgnoreCase)
            || currentUrl.Equals("Default/Kickout", StringComparison.OrdinalIgnoreCase);

        private bool IsForceChangePassword
            => !UserSession.Current.IsLogged || UserSession.Current.IsPasswordExpired;

        private static string ForceChangePasswordUrl(string currentUrl)
            => currentUrl.Equals("Password/ForceChangePassword", StringComparison.OrdinalIgnoreCase) ||
            currentUrl.Equals("Password/SkipForceChangePassword", StringComparison.OrdinalIgnoreCase)
            ? null
            : MicroSites.MainUrl("~/Password/ForceChangePassword");

        private static string ForceChangeSecurityCodeUrl(string currentUrl, string securityCode)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
            {
                return currentUrl.Equals("SecurityCode/ChangeSecurityCodeFirstLogin", StringComparison.OrdinalIgnoreCase)
                    ? null
                    : "~/SecurityCode/ChangeSecurityCodeFirstLogin";
            }

            return currentUrl.Equals("SecurityCode/ChangeSecurityCodeByForcing", StringComparison.OrdinalIgnoreCase)
                ? null
                : "~/SecurityCode/ChangeSecurityCodeByForcing";
        }

        private bool IsForceLoginByNickname
        {
            get
            {
                NicknameChecker nicknameChecker = new NicknameCheckerImpl(userSession: UserSession.Current);

                return nicknameChecker.IsForceLoginByNickname;
            }
        }

        private static string CreateNickNameUrl(string currentUrl)
            => currentUrl.Equals("Nickname/Index", StringComparison.OrdinalIgnoreCase)
            ? null
            : MicroSites.MainUrl("~/Nickname/Index");
    }
}