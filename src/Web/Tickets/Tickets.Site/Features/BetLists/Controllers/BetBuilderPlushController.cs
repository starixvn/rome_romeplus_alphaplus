using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetBuilderPlus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class BetBuilderPlusController(IMediator mediator) : BetListBaseController(mediator)
{
    public async Task<IActionResult> BetBuilderPlus(BetBuilderPlusQuery message)
    {
      return Content(await mediator.Send(message));
    }
}
