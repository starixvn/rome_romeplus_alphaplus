using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class WinLossController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(WinLossQuery message) => View(message);

    public Task<IActionResult> Excel(WinLossExcelQuery message) => ExcelView(message);

    public Task<IActionResult> CashFee(CashFeeGetDataQuery message) => View<CashFeeViewModel>(message);

    public Task<IActionResult> CashFeeExcel(CashFeeExcelQuery message) => ExcelView(message);
}