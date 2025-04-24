using AlphaPlus.Tickets.Site.Features.BetLists.Requests.DailyStatement;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class DailyStatementController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(DailyStatementQuery message)
        => View(message, GetViewPath("DailyStatement", nameof(Index)));

    public Task<FileContentResult> Excel(DailyStatementExcelQuery message) => ExcelView(message);
}