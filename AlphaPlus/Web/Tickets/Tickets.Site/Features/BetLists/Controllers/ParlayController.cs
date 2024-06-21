using AlphaPlus.Tickets.Site.Features.BetLists.Requests.Parlay;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class ParlayController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> MixParlay(MixParlayQuery message)
        => PartialView(message, string.Format(BetListView, "Parlay", nameof(MixParlay)));

    public Task<IActionResult> ComboParlay(ComboParlayQuery message)
        => PartialView(message, string.Format(BetListView, "Parlay", nameof(ComboParlay)));

    public Task<IActionResult> ComboParlayDetail(ComboParlayDetailQuery message)
        => View(message, string.Format(BetListView, "Parlay", nameof(ComboParlayDetail)));

    public Task<FileContentResult> ComboParlayDetailExcel(ComboParlayDetailExcelQuery message)
        => ExcelView(message);
}