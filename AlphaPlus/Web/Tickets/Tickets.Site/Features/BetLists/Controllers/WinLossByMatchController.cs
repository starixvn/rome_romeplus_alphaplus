using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLossByMatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class WinLossByMatchController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(WinLossByMatchQuery message)
        => View(message, GetViewPath("WinLossByMatch", nameof(Index)));

    public Task<FileContentResult> Excel(WinLossByMatchExcelQuery message)
        => ExcelView(message);
}