using Microsoft.AspNetCore.Mvc;
using Alpha.Main.Site.Requests.Banner;
using MediatR;
using AlphaPlus.Core;
using AlphaPlus.Core.Filters;

namespace Alpha.Main.Site.Controllers
{
    public class BannerController(IMediator mediator) : BaseController
    {
        private readonly IMediator mediator = mediator;

        [AllowProtectedMode]
        public async Task<IActionResult> Index(BannerQuery request)
        {
            var viewModel = request.IsHideBanner
                ? null
                : await mediator.Send(request);

            return PartialView(viewModel);
        }
    }
}