using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.Core;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.Settings.Site.Transfer.Controllers
{
    public class TransferAllClearCreditControllerBase : BaseController
    {
        protected readonly IMediator mediator;
        protected const string InvalidPageViewPath = "~/Views/Shared/InvalidPage.cshtml";

        protected TransferAllClearCreditControllerBase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected ActionResult View<TResponse>(IRequest<TResponse> message)
        {
            return View(mediator.Send(message));
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var userSession = UserSession.Current;
            if (SubAccDoesNotHavePermission(userSession) || NotOfferTransferAllAndClearCredit(userSession))
            {
                filterContext.Result = new UnauthorizedObjectResult("You are not authorized on this function!");

                var session = filterContext.HttpContext.Session;
                session.Clear();
            }
        }

        private static bool SubAccDoesNotHavePermission(IUserSession userSession)
        {
            return userSession.IsSubAcc && !userSession.SubPageAccess.Transfer;
        }

        private static bool NotOfferTransferAllAndClearCredit(IUserSession userSession)
        {
            return false;
            //TODO return !AppSettings.OfferClearGivenCreditSites.Any(s => string.Equals(s, "*"))
            //     && !AppSettings.OfferClearGivenCreditSites.Any(s => string.Equals(s, userSession.Site, StringComparison.OrdinalIgnoreCase));
        }
    }
}