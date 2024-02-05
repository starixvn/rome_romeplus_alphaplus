using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;

namespace Alpha.Main.Site.Features.UMMessage
{
    public class UMMessageController(IMediator mediator) : BaseController
    {
        private readonly IMediator mediator = mediator;

        [AllowProtectedMode]
        public async Task<IActionResult> Get(UMMessageQuery request)
        {
            var messages = request.EnableUMAnnouncementPopup
                ? await mediator.Send(request)
                : [];

            return Json(messages);
        }
    }
}