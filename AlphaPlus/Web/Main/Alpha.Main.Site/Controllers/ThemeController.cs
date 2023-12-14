using Microsoft.AspNetCore.Mvc;
using Alpha.Main.Site.Requests.Theme;
using MediatR;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;

namespace Alpha.Main.Site.Controllers
{
    public class ThemeController(IMediator mediator) : BaseController
    {
        private readonly IMediator mediator = mediator;

        [HttpPost]
        [AllowProtectedMode]
        public async Task<IActionResult> Change(ChangeThemeQuery query)
            => Json(await mediator.Send(query));

        [HttpPost]
        [AllowProtectedMode]
        public async Task<IActionResult> ResetAssetAppPath(ResetAssetAppPathQuery query)
            => Json(await mediator.Send(query));
    }
}