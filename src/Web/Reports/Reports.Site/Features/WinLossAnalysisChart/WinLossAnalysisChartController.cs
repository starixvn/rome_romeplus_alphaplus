using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysisChart;

public class WinLossAnalysisChartController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(WinLossAnalysisChartQuery message) => View(message);
}