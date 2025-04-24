using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public class EnteredResultController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(EnteredResultQuery message) => View(message);

    public Task<IActionResult> Excel(EnteredResultExcelQuery message) => ExcelView(message);
}