using AlphaPlus.Core.Filters;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

[AllowProtectedMode]
public class CommissionByBetTypeController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(CommissionByBetTypeQuery message) => View(message);

    public Task<IActionResult> Excel(CommissionByBetTypeExcelQuery message) => ExcelView(message);
}