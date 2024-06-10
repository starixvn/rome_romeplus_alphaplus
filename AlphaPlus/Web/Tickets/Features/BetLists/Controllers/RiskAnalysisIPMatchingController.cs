using AlphaPlus.Tickets.Site.Features.BetLists.Requests.RiskAnalysisIPMatching;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class RiskAnalysisIPMatchingController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(RiskAnalysisIPMatchingQuery message)
        => View(message, GetViewPath("RiskAnalysisIPMatching", nameof(Index)));

    public Task<FileContentResult> Excel(RiskAnalysisIPMatchingExcelQuery message)
        => ExcelView(message);
}
