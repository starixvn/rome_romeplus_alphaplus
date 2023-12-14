using Microsoft.AspNetCore.Mvc;
using Alpha.Main.AppResources;
using Alpha.Main.Models;
using Alpha.Main.Site.Requests.ProtectionCode;
using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.Core;

namespace Alpha.Main.Site.Controllers
{
    public class ProtectionCodeController(IMediator mediator, IUserSession userSession) : BaseController
    {
        private readonly IMediator mediator = mediator;
        private readonly IUserSession userSession = userSession;

        public async Task<IActionResult> Setup(ProtectionCodeQuery query)
        {
            if (query.IsOfferProtectedMode)
            {
                return View(await mediator.Send(query));
            }

            return RedirectToAction("Index", "Default");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Setup(UpdateProtectionCodeCommand query)
        {
            var result = query.IsInternal || query.IsNotOfferProtectedMode
                ? Result.CreateErrorResult(Security.NotAuthorized)
                : await mediator.Send(query);

            return Json(result);
        }
    }
}