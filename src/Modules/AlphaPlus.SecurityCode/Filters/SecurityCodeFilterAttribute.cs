using AlphaPlus.SecurityCode.Services;
using AlphaPlus.SecurityCode.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.SecurityCode.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class SecurityCodeFilterAttribute : TypeFilterAttribute
{
    public SecurityCodeFilterAttribute() : base(typeof(SecurityCodeFilter))
    {
    }

    private class SecurityCodeFilter(
        ISecurityCodeChecker securityCodeChecker,
        ISecurityCodeHttpContext securityCodeHttpContext) : IAsyncActionFilter
    {
        private readonly ISecurityCodeChecker securityCodeChecker = securityCodeChecker;
        private readonly ISecurityCodeHttpContext securityCodeHttpContext = securityCodeHttpContext;

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!securityCodeHttpContext.IsValidRequest())
            {
                return Task.CompletedTask;
            }

            var relativePath = securityCodeHttpContext.GetRelativePath();
            var isRequiredSecurityCode = securityCodeChecker.IsRequiredSecurityCode(relativePath);

            if (isRequiredSecurityCode)
            {
                if (securityCodeHttpContext.IsAjaxRequest())
                {
                    return securityCodeHttpContext.DenyAjaxRequest();
                }
                else
                {
                    securityCodeHttpContext.RedirectToVerifySecurityCodePage();
                    return Task.CompletedTask;
                }
            }

            return next();
        }
    }
}