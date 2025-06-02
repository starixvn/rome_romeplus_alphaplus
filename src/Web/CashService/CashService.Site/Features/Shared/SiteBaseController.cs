using AlphaPlus.CashService.Site.Features.Shared.Filters;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.CashService.Site.Features.Shared
{
    [SiteAuthorizationFilter]
    [AutoValidateAntiforgeryToken]
    public class SiteBaseController : BaseController
    {
        protected readonly IUserSession _userSession;
        protected readonly IAntiforgery _antiforgery;
        
        protected SiteBaseController(IUserSession userSession, IAntiforgery antiforgery)
        {
            _userSession = userSession;
            _antiforgery = antiforgery;
        }


        protected string GetAntiforgeryToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            return tokens.RequestToken;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["RequestVerificationToken"] = GetAntiforgeryToken();
        }
    }
}