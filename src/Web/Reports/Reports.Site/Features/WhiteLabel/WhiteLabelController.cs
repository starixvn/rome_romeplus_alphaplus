using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class WhiteLabelController(IMediator mediator, IConfiguration configuration) : ReportBaseController(mediator)
{
    private readonly IConfiguration configuration = configuration;

    #region Settlement
    public Task<IActionResult> Settlement(SettlementQuery message) => View<SettlementViewModel>(message);

    public Task<IActionResult> GetSettlementData(SettlementGetDataQuery message)
        => View(message, "~/Features/WhiteLabel/Views/SettlementData.cshtml");

    public Task<IActionResult> SettlementExcel(SettlementExcelQuery message) => ExcelView(message);
    #endregion

    #region DailyWinLoss
    public Task<IActionResult> DailyWinLoss(DailyWinLossQuery message) => View<DailyWinLossViewModel>(message);

    public Task<IActionResult> GetDailyWinLossData(DailyWinLossGetDataQuery message)
        => View(message, "~/Features/WhiteLabel/Views/DailyWinLossData.cshtml");

    public Task<IActionResult> DailyWinLossExcel(DailyWinLossExcelQuery message) => ExcelView(message);
    #endregion

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (!configuration["WhiteLabel:OfferedSites"].ToList().Contains(UserSession.Current.Site, StringComparer.OrdinalIgnoreCase))
        {
            context.Result = new RedirectResult("/reports/shared/invalidreport");
        }
    }
}