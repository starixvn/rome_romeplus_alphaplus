using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Requests;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines
{
  public class OpenDownlinesController(IMediator mediator) : SiteBaseController(mediator)
  {
    public async Task<ActionResult> EditSingle(OpenSingleRequest message)
      => await View(message).ConfigureAwait(false);

    public async Task<ActionResult> EditMultiple(OpenMultipleRequest message)
      => await View(message).ConfigureAwait(false);

    public async Task<ActionResult> FetchCustomerList(OpenViewListRequest message)
      => await Json(message).ConfigureAwait(false);

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<ActionResult> OpenSingle([FromBody] OpenSingleCommand command)
      => await Json(command).ConfigureAwait(false);

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<ActionResult> OpenMultiple([FromBody] OpenMultipleCommand command)
      => await Json(command).ConfigureAwait(false);
  }
}