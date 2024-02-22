using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;

namespace AlphaPlus.Main.Site.Features.Theme
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