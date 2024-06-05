using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CombinationMixParlay;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CombinationMixParlay;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class CombinationMixParlayController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(CombinationMixParlayQuery message)
        => View<CombinationMixParlayViewModel>(message, string.Format(BetListView, "CombinationMixParlay", nameof(Index)));

    public Task<FileContentResult> Excel(CombinationMixParlayExcelQuery message)
        => ExcelView(message);
}