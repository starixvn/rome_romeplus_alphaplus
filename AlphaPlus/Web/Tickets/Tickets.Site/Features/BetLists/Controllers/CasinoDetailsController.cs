using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class CasinoDetailsController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(CasinoDetailsQuery message)
        => View(message, GetViewPath("CasinoDetails", nameof(Index)));

    public Task<FileContentResult> Excel(CasinoDetailsExcelQuery message) => ExcelView(message);
}
