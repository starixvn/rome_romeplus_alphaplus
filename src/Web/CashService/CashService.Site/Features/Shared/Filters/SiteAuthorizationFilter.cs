using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.CashService.Site.Features.Shared.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SiteAuthorizationFilterAttribute : TypeFilterAttribute
    {
        public SiteAuthorizationFilterAttribute(string pageAccessType) : base(typeof(SiteAuthorizationFilter))
        {
            Arguments = [pageAccessType];
        }

        public class SiteAuthorizationFilter(IUserSession userSession, string pageAccessType) : IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!IsValidSession())
                {
                    context.Result = new UnauthorizedObjectResult("You are not authorized on this function.");
                }
            }

            private bool IsValidSession()
            {
                if (userSession.CustId <= 0)
                {
                    return false;
                }

                return AppSettings.IsOfferedCashService(userSession.Site)
                    && (!userSession.IsSubAcc || userSession.SubPageAccess.GetPermission(pageAccessType));
            }
        }
    }
}