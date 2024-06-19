using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CancelledBets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class CancelledBetsController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(CancelledBetQuery message)
        => View(message, GetViewPath("CancelledBets", nameof(Index)));

    public Task<FileContentResult> Excel(CancelledBetExcelQuery message) => ExcelView(message);
}