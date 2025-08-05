using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.ViewLogs.Helpers;
using MediatR;
using AlphaPlus.Settings.Site.Features.ViewLogs.Requests.CashService;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Controllers
{
    public class CashServiceController : ViewLogBaseController
    {
        private const string TemplatePath = "App_Data/TrackingValueTransformTemplates";
        private readonly IMediator mediator;

        public CashServiceController(
            IUserSession userSession,
            IMediator mediator,
            MicroSites microSites) : base(userSession, microSites)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Index(IndexQuery indexQuery)
        {
            if (!AlphaPlus.Core.Configurations.AppSettings.IsOfferedCashService(UserSession.Site))
            {
                return RedirectToAction("Index", "Settings");
            }

            indexQuery.TrackingValueTransformTemplatePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{TemplatePath}";

            var viewModel = await mediator.Send(indexQuery);

            return View(viewModel);
        }
    }
}