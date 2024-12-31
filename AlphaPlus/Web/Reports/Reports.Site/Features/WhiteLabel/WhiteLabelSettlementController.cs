using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class WhiteLabelSettlementController(IMediator mediator, IConfiguration configuration) : ReportBaseController(mediator)
{
    private readonly IConfiguration configuration = configuration;

    public Task<IActionResult> Index(SettlementQuery message)
        => View<SettlementViewModel>(message, "~/Features/WhiteLabel/Views/Settlement.cshtml");

    public Task<IActionResult> GetData(SettlementGetDataQuery message)
        => View(message, "~/Features/WhiteLabel/Views/SettlementData.cshtml");

    public Task<IActionResult> Excel(SettlementExcelQuery message) => ExcelView(message);

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (!configuration["WhiteLabel:OfferedSites"].ToList().Contains(UserSession.Current.Site, StringComparer.OrdinalIgnoreCase))
        {
            context.Result = new RedirectResult("/reports/shared/invalidreport");
        }
    }
}