using Microsoft.AspNetCore.Mvc;
using Alpha.Main.Models.UMMessage;
using Alpha.Main.Site.Requests.UMMessage;
using MediatR;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;

namespace Alpha.Main.Site.Controllers
{
    public class UMMessageController(IMediator mediator) : BaseController
    {
        private readonly IMediator mediator = mediator;

        [AllowProtectedMode]
        public async Task<IActionResult> Get(UMMessageQuery request)
        {
            var messages = request.EnableUMAnnouncementPopup
                ? await mediator.Send(request)
                : Enumerable.Empty<UMMessageDTO>();

            return Json(messages);
        }
    }
}