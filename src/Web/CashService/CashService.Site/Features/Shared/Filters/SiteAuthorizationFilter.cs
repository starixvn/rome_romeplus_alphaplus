using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.CashService.Site.Features.Shared.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SiteAuthorizationFilterAttribute : TypeFilterAttribute
    {
        public SiteAuthorizationFilterAttribute() : base(typeof(SiteAuthorizationFilter))
        {
        }

        public class SiteAuthorizationFilter(IUserSession userSession) : IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if(!IsValidSession())
                {
                    context.Result = new UnauthorizedObjectResult("You are not authorized on this function.");
                }
            }

            private bool IsValidSession()
            {
                if(userSession.CustId <= 0)
                {
                    return false;
                }

                return AppSettings.IsOfferedCashService(userSession.Site) 
                    && (!userSession.IsSubAcc || userSession.SubPageAccess.GetPermission(PageAccessType.CreateAccount));
            }
        }
    }
} 