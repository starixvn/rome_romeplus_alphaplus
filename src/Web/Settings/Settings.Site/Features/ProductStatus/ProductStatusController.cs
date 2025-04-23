using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.ProductStatus.Requests;
using AlphaPlus.Settings.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.ProductStatus
{
    public class ProductStatusController(IMediator mediator)
        : SiteBaseController(mediator)
    {
        public async Task<ActionResult> Index(ProductStatusQuery message)
            => await View(message);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(ProductStatusCommand message)
            => await Json<IEnumerable<Result>>(message);
    }
}