using AlphaPlus.SecurityCode.Services;
using AlphaPlus.SecurityCode.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.SecurityCode.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class SecurityCodeFilterAttribute : TypeFilterAttribute
{
    public SecurityCodeFilterAttribute()
        : base(typeof(SecurityCodeFilter))
    {
    }

    private class SecurityCodeFilter(
        ISecurityCodeChecker securityCodeChecker,
        ISecurityCodeHttpContext securityCodeHttpContext)
        : IActionFilter
    {
        private readonly ISecurityCodeChecker securityCodeChecker = securityCodeChecker;
        private readonly ISecurityCodeHttpContext securityCodeHttpContext = securityCodeHttpContext;

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!securityCodeHttpContext.IsValidRequest())
            {
                return;
            }

            var relativePath = securityCodeHttpContext.GetRelativePath();
            var isRequiredSecurityCode = securityCodeChecker.IsRequiredSecurityCode(relativePath);

            if (isRequiredSecurityCode)
            {
                if (securityCodeHttpContext.IsAjaxRequest())
                {
                    securityCodeHttpContext.DenyAjaxRequest();
                }
                else
                {
                    securityCodeHttpContext.RedirectToVerifySecurityCodePage();
                }
            }
        }
    }
}


