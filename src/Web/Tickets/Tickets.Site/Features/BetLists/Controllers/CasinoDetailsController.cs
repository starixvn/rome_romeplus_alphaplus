using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class CasinoDetailsController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> BBinIndex(CasinoDetailsQuery message)
        => View(message, GetViewPath("CasinoDetails", nameof(Index)));

    public Task<FileContentResult> BBinExcel(CasinoDetailsExcelQuery message) => ExcelView(message);

    public Task<IActionResult> LiveCasino(LiveCasinoDetailsQuery message)
        => View(message, GetViewPath("CasinoDetails", nameof(Index)));

    public Task<FileContentResult> LiveCasinoExcel(LiveCasinoDetailsExcelQuery message) => ExcelView(message);

    public Task<IActionResult> RNG(RNGCasinoDetailsQuery message)
        => View(message, GetViewPath("CasinoDetails", nameof(Index)));

    public Task<FileContentResult> RNGExcel(RNGCasinoDetailsExcelQuery message) => ExcelView(message);
}
