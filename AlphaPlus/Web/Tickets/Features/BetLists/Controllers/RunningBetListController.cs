using AlphaPlus.Tickets.Site.Features.BetLists.Requests.RunningBetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class RunningBetListController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(RunningBetListQuery message)
        => View(message, GetViewPath("RunningBetList", nameof(Index)));
}
