using AlphaPlus.Tickets.Site.Features.BetLists.Requests.MemberBetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class MemberBetListController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(MemberBetListQuery message)
        => View(message, GetViewPath("MemberBetList", nameof(Index)));
}