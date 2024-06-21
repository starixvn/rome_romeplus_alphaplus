using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class WinLossController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(WinLossQuery message)
        => View(message, GetViewPath("WinLoss", nameof(Index)));

    public Task<FileContentResult> Excel(WinLossExcelQuery message) => ExcelView(message);
}