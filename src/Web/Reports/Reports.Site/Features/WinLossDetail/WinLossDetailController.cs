using AlphaPlus.Core.Filters;
using AlphaPlus.Reports.Features.WinLossDetail.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.WinLossDetail.Requests;
using AlphaPlus.Reports.Site.Features.WinLossDetail.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

[AllowProtectedMode]
public class WinLossDetailController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(WinLossDetailQuery message)
        => 
        View<WinLossDetailViewModel>(message);

    public Task<IActionResult> GetData(WinLossDetailGetDataQuery message)
        => View<WinLossDetailGetDataViewModel>(message, "ReportData");

    public Task<IActionResult> GetWeeks(WinLossDetailGetWeeksQuery message) => View(message, "Weeks");

    public Task<IActionResult> Excel(WinLossDetailExcelQuery message) => ExcelView(message);

    public Task<IActionResult> CashFee(CashFeeQuery message) => View<CashFeeViewModel>(message);

    public Task<IActionResult> GetCashFeeData(CashFeeGetDataQuery message)
       => View<CashFeeModel>(message, "CashFeeData");

    public Task<IActionResult> CashFeeExcel(CashFeeExcelQuery message) => ExcelView(message);
}