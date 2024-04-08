using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> ConsolidationByUsername(ConsolidationByUsernameQuery message) => View(message);

    public Task<IActionResult> ConsolidationByUsernameExcel(ConsolidationByUsernameExcelQuery message) => ExcelView(message);

    public Task<IActionResult> Index(ConsolidationByGroupQuery message) => View(message);

    public Task<IActionResult> Excel(ConsolidationByGroupExcelQuery message) => ExcelView(message);

    public Task<IActionResult> ConsolidationDetail(ConsolidationDetailQuery message) => View(message);
}