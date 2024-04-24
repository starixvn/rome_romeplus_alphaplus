namespace AlphaPlus.Settings.Site.ViewLogs.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Core.Models;
    using Helpers;
    using MediatR;
    using Requests.Setting;

    public class SettingController : ViewLogBaseController
    {
        private const string TemplatePath = "App_Data/TrackingValueTransformTemplates";
        private readonly IMediator mediator;

        public SettingController(
            IUserSession userSession,
            IMediator mediator,
            MicroSites microSites) : base(userSession, microSites)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Index(IndexQuery indexQuery)
        {
            indexQuery.TrackingValueTransformTemplatePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{TemplatePath}";

            var viewModel = await mediator.Send(indexQuery);

            return View(viewModel);
        }
    }
}