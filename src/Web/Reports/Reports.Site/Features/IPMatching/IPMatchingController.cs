using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public class IPMatchingController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(IPMatchingQuery message) => View(message);

    public Task<IActionResult> Excel(IPMatchingExcelQuery message) => ExcelView(message);
}