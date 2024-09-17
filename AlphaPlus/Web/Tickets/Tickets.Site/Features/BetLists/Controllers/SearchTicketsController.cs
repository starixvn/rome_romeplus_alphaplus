using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.SearchTickets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.SearchTickets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class SearchTicketsController(IMediator mediator) : BetListBaseController(mediator)
{
    public IActionResult Index(SearchTicketsQuery message)
    {
        var viewModel = new SearchTicketsViewModel(message, new BetListData(null, null), null, null);

        return View(GetViewPath("SearchTickets", nameof(Index)), viewModel);
    }

    public Task<IActionResult> BetData(SearchTicketsQuery message)
        => View(message, GetViewPath("Shared", "WinLossTableNew"));
}