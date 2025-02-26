using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.ViewLogs.Helpers;
using MediatR;
using AlphaPlus.Settings.Site.Features.ViewLogs.Requests.Credit;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Controllers
{
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

        public async Task<ActionResult> Index(IndexQuery indexQuery)
        {
            var viewModel = await mediator.Send(indexQuery);

            return View(viewModel);
        }
    }
}