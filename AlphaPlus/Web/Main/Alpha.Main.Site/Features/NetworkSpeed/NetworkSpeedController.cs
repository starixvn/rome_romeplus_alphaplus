using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using MediatR;
using Alpha.Main.Site.Requests.NetworkSpeed;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;

namespace Alpha.Main.Site.Features.NetworkSpeed
{
    [AllowProtectedMode]
    public class NetworkSpeedController(IMediator mediator, IUserSession userSession) : BaseController
    {
        private readonly IMediator mediator = mediator;
        private readonly IUserSession userSession = userSession;

        public async Task<IActionResult> Index()
        {
            return View(await mediator.Send(new NetworkSpeedQuery(userSession.CustId)));
        }

        [HttpGet]
        public async Task<JsonResult> SaveResult(SaveNetworkSpeedCommand command)
        {
            await mediator.Send(command);

            return Json("ok");
        }
    }
}