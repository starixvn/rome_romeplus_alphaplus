using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public class MatchWinLossDetailController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(MatchWinLossDetailQuery message) => View(message);

    public Task<IActionResult> Leagues(MatchWinLossDetailGetLeagueQuery message) => View(message);

    public Task<IActionResult> Excel(MatchWinLossDetailExcelQuery message) => ExcelView(message);
}