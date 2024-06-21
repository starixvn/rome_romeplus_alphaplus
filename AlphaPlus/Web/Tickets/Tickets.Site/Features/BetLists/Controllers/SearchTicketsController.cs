using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.SearchTickets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.SearchTickets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class SearchTicketsController(IMediator mediator) : BetListBaseController(mediator)
{
    public IActionResult Index(SearchTicketsQuery message)
        => View(new SearchTicketsViewModel(message, new BetListData(null, null), null, null));

    public Task<IActionResult> BetData(SearchTicketsQuery message)
        => View(message, string.Format(BetListView, "Shared", "WinLossTableNew"));
}