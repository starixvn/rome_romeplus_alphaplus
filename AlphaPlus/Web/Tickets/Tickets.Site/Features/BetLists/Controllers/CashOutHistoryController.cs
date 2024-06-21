using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CashOutHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class CashOutHistoryController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(CashOutHistoryQuery message)
        => View(message, string.Format(BetListView, "CashOutHistory", nameof(Index)));
}