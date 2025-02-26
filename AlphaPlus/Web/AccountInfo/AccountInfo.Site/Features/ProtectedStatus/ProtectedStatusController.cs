using AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Requests;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus
{
    public class ProtectedStatusController : SiteBaseController
    {
        public ProtectedStatusController(IMediator mediator)
        : base(mediator)
        {
        }

        public async Task<ActionResult> EditMultiple(EditMultipleRequest message)
           => await View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMultiple([FromBody] EditMultipleCommand command)
            => await Json(command);
    }
}