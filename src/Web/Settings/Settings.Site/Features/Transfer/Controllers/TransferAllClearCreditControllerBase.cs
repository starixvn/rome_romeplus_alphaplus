using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.Core;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers
{
    public class TransferAllClearCreditControllerBase : BaseController
    {
        protected readonly IMediator mediator;
        protected const string InvalidPageViewPath = "~/Features/Transfer/Views/Shared/InvalidPage.cshtml";

        protected TransferAllClearCreditControllerBase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message)
        {
            return View(await mediator.Send(message));
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var userSession = UserSession.Current;
            if (SubAccDoesNotHavePermission(userSession) || !OfferTransferAllAndClearCredit(userSession))
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

        private static bool OfferTransferAllAndClearCredit(IUserSession userSession)
        {
            var offerClearGivenCreditSites = Core.Configurations.ConfigurationManager.GetValue<string>("OfferClearGivenCredit:Sites")
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                return offerClearGivenCreditSites.Any(s
                    => string.Equals(s, userSession.Site, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(s, "*", StringComparison.OrdinalIgnoreCase));
        }
    }
}