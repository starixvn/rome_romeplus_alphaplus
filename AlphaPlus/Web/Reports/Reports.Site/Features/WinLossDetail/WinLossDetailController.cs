using AlphaPlus.Core.Filters;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

[AllowProtectedMode]
public class WinLossDetailController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(WinLossDetailQuery message) => View(message);

    public Task<IActionResult> Excel(WinLossDetailExcelQuery message) => ExcelView(message);

    public Task<IActionResult> IndexNew(WinLossDetailNewQuery message) => View(message);

    public Task<IActionResult> GetData(WinLossDetailGetDataQuery message)
        => Json<WinLossDetailGetDataViewModel>(message);

    public Task<IActionResult> GetWeeks(WinLossDetailGetWeeksQuery message)
        => Json<IEnumerable<WinLossDetailWeekViewModel>>(message);

    public Task<IActionResult> ExcelNew(WinLossDetailExcelNewQuery message) => ExcelView(message);
}