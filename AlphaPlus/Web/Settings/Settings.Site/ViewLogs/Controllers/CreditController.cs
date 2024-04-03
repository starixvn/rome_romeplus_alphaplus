namespace AlphaPlus.Settings.Site.ViewLogs.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Core.Models;
    using Helpers;
    using MediatR;
    using Requests.Credit;

    public class CreditController : ViewLogBaseController
    {
        private readonly IMediator mediator;

        public CreditController(
            IUserSession userSession,
            IMediator mediator,
            MicroSites microSites) : base(userSession, microSites)
        {
            this.mediator = mediator;
        }

        public ActionResult Index(IndexQuery indexQuery)
        {
            var viewModel = mediator.Send(indexQuery);

            return View(viewModel);
        }
    }
}