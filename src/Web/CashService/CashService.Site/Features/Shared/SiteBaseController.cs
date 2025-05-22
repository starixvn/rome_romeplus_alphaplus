using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Shared
{
    public class SiteBaseController : BaseController
    {
        private const string InvalidPageViewPath = "~/Features/Shared/Views/Invalid.cshtml";
        protected readonly IUserSession _userSession;

        protected SiteBaseController(IUserSession userSession)
        {
            _userSession = userSession;
        }

        protected ActionResult SiteView<TResponse>(TResponse response)
            => IsValidSession()
                ? View(response)
                : View(InvalidPageViewPath);

        protected ActionResult SiteView<TResponse>(TResponse response, string viewName)
            => IsValidSession()
                ? View(viewName, response)
                : View(InvalidPageViewPath);

        protected ActionResult SiteJson<TResponse>(TResponse response)
            => IsValidSession() ?
                Json(response) :
                Json(Result.CreateErrorResult("You are not authorized on this function.", StatusCodes.Status403Forbidden));

        public bool IsValidSession()
        {
            if(_userSession.CustId <= 0)
            {
                return false;
            }

            return AppSettings.IsOfferedCashService(_userSession.Site) 
                && (!_userSession.IsSubAcc || _userSession.SubPageAccess.GetPermission(PageAccessType.CreateAccount));
        }
    }
}