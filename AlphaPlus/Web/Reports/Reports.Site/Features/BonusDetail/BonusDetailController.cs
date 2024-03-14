using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.BonusDetail;

public class BonusDetailController(
    IMediator mediator,
    IOptions<AppSettings> options) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(BonusDetailQuery message) => View(message);

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> ConfirmPayment(ConfirmPaymentCommand message) => Json(message);

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (!options.Value.AllowedSitesToViewBonusDetail.Contains(UserSession.Current.Site))
        {
            context.Result = new RedirectResult("/tickets/home/invalidreport");
        }
    }
}
