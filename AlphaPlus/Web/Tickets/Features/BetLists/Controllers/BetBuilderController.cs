using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class BetBuilderController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> ViewDetail(BetBuilderDetailQuery message)
        => View(message, string.Format(BetListView, "BetBuilder", nameof(ViewDetail)));
}
