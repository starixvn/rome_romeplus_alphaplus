using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> ConsolidationByUsername(ConsolidationByUsernameQuery message) => View(message);

    public Task<IActionResult> ConsolidationByDate(ConsolidationByDateQuery message) => View(message);

    public Task<IActionResult> ConsolidationByProduct(ConsolidationByProductQuery message) => View(message);

    public Task<IActionResult> ConsolidationExcel(ConsolidationExcelQuery message) => ExcelView(message);

    public Task<IActionResult> ConsolidationDetail(ConsolidationDetailQuery message) => View(message);

    public Task<IActionResult> ConsolidationDetailExcel(ConsolidationDetailExcelQuery message) => ExcelView(message);
}