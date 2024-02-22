using Microsoft.AspNetCore.Mvc;
using Alpha.Main.Site.AppResources;
using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.Core;

namespace Alpha.Main.Site.Features.ProtectionCode
{
    public class ProtectionCodeController(IMediator mediator) : BaseController
    {
        private readonly IMediator mediator = mediator;

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