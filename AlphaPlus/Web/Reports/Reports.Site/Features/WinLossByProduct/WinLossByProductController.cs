using AlphaPlus.Core.Filters;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.WinLossByProduct;

[AllowProtectedMode]
public class WinLossByProductController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(WinLossByProductQuery message) => View(message);

    public Task<IActionResult> Excel(WinLossByProductExcelQuery message) => ExcelView(message);
}