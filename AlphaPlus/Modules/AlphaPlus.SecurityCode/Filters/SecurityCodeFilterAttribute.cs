using AlphaPlus.SecurityCode.Services;
using AlphaPlus.SecurityCode.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.SecurityCode.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class SecurityCodeFilterAttribute : TypeFilterAttribute
{
    public SecurityCodeFilterAttribute(bool isBetLists = false, bool isStatement = false)
        : base(typeof(SecurityCodeFilter))
    {
        Arguments = [isBetLists, isStatement];
    }

    private class SecurityCodeFilter(
        ISecurityCodeChecker securityCodeChecker,
        ISecurityCodeHttpContext securityCodeHttpContext,
        bool isBetLists,
        bool isStatement)
        : IActionFilter
    {
        private readonly ISecurityCodeChecker securityCodeChecker = securityCodeChecker;
        private readonly ISecurityCodeHttpContext securityCodeHttpContext = securityCodeHttpContext;
        private readonly bool isBetLists = isBetLists;
        private readonly bool isStatement = isStatement;

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!securityCodeHttpContext.IsValidRequest())
            {
                return;
            }

            var isRequiredSecurityCode = securityCodeChecker.IsRequiredSecurityCode(isBetLists, isStatement);

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


