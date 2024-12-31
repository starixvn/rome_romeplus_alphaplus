using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class WhiteLabelDailyWinLossController(IMediator mediator, IConfiguration configuration) : ReportBaseController(mediator)
{
    private readonly IConfiguration configuration = configuration;

    public Task<IActionResult> Index(DailyWinLossQuery message)
        => View<DailyWinLossViewModel>(message, "~/Features/WhiteLabel/Views/DailyWinLoss.cshtml");

    public Task<IActionResult> GetData(DailyWinLossGetDataQuery message)
        => View(message, "~/Features/WhiteLabel/Views/DailyWinLossData.cshtml");

    public Task<IActionResult> Excel(DailyWinLossExcelQuery message) => ExcelView(message);

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (!configuration["WhiteLabel:OfferedSites"].ToList().Contains(UserSession.Current.Site, StringComparer.OrdinalIgnoreCase))
        {
            context.Result = new RedirectResult("/reports/shared/invalidreport");
        }
    }
}