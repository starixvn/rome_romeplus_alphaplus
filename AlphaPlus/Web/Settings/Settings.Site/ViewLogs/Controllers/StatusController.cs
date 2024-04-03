namespace AlphaPlus.Settings.Site.ViewLogs.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Core.Models;
    using Helpers;
    using MediatR;
    using Requests.Status;

    public class StatusController : ViewLogBaseController
    {
        private const string TemplatePath = "/app/ViewLogs/App_Data/TrackingValueTransformTemplates";
        private readonly IMediator mediator;

        public StatusController(
            IUserSession userSession,
            IMediator mediator,
            MicroSites microSites) : base(userSession, microSites)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Index(IndexQuery indexQuery)
        {
            indexQuery.TrackingValueTransformTemplatePath = TemplatePath;
            var viewModel = await mediator.Send(indexQuery);

            return View(viewModel);
        }
    }
}