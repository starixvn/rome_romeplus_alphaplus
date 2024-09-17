using AlphaPlus.Tickets.Site.Features.BetLists.Requests.ParlayWinLossByMatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class ParlayWinLossByMatchController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(ParlayWinLossByMatchQuery message)
        => View(message, GetViewPath("ParlayWinLossByMatch", nameof(Index)));

    public Task<FileContentResult> Excel(ParlayWinLossByMatchExcelQuery message) => ExcelView(message);
}
