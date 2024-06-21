using AlphaPlus.Tickets.Site.Features.BetLists.Requests.JackpotDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class JackpotDetailsController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(JackpotDetailsQuery message)
        => View(message, GetViewPath("JackpotDetails", nameof(Index)));

    public Task<FileContentResult> Excel(JackpotDetailsExcelQuery message)
        => ExcelView(message);
}