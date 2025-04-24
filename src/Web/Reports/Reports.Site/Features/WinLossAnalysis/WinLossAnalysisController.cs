using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysis;

public class WinLossAnalysisController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(WinLossAnalysisQuery message) => View(message);

    public Task<IActionResult> Excel(WinLossAnalysisExcelQuery message) => ExcelView(message);
}