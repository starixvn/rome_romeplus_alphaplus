using AlphaPlus.Tickets.Site.Features.BetLists.Requests.MoveStatementHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class MoveStatementHistoryController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(MoveStatementHistoryQuery message)
        => View(message, string.Format(BetListView, "MoveStatementHistory", nameof(Index)));
}
