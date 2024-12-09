using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines
{
    public class UnsuspendDownlinesController : SiteBaseController
    {
        public UnsuspendDownlinesController(IMediator mediator)
            : base(mediator)
        {
        }

        public Task<ActionResult> EditSingle(UnsuspendSingleRequest message)
            => View(message);
        public Task<ActionResult> EditMultiple(UnsuspendMultipleRequest message)
            => View(message);

        public Task<ActionResult> FetchCustomerList(UnsuspendViewListRequest message)
           => Json(message);

        [HttpPost, ValidateAntiForgeryToken]
        public Task<ActionResult> UnsuspendSingle(UnsuspendSingleCommand command)
          => Json(command);

        [HttpPost, ValidateAntiForgeryToken]
        public Task<ActionResult> UnsuspendMultiple(UnsuspendMultipleCommand command)
          => Json(command);

        [HttpGet]
        public Task<ActionResult> MultipleSuspendConfirm(SuspendMultipleConfirmRequest message)
            => View(message);
    }
}