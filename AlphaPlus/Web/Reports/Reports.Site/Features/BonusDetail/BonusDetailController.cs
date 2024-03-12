using System.Net;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Result = AlphaPlus.Reports.Features.Shared.Models.Result;

namespace AlphaPlus.Reports.Site.Features.BonusDetail;

public class BonusDetailController(
    IMediator mediator,
    IOptions<AppSettings> options) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(BonusDetailQuery message) => View(message);

    [HttpPost]
    public async Task<ActionResult> ConfirmPayment(ConfirmPaymentCommand message)
    {
        var result = message.IsValid()
            ? await mediator.Send(message)
            : new Result((int)HttpStatusCode.Forbidden, Resources.NotAuthorized);

        return Json(result);
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (!options.Value.AllowedSitesToViewBonusDetail.Contains(UserSession.Current.Site))
        {
            context.Result = new RedirectResult("/tickets/home/invalidreport");
        }
    }
}