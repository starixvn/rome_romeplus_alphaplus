using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;

public partial class WinLossLimitController(IMediator mediator, IUserSession userSession, IAntiforgery antiforgery) 
    : SiteBaseController(mediator)
{
    public Task<ActionResult> Index(WinLossLimitQuery message)
    {	
        var tokens = antiforgery.GetAndStoreTokens(HttpContext);
        ViewData["RequestVerificationToken"] = tokens.RequestToken;	
        return View(message, "~/Features/MemberInfo/Views/WinLossLimit/Index.cshtml");
    }

    public Task<ActionResult> SportsbookPotentialWinLimit(SportsbookPotentialWinLimitQuery message)
        => View(message, "~/Features/MemberInfo/Views/WinLossLimit/SportsbookPotentialWinLimit.cshtml");

    public async Task<ActionResult> PotentialExcel(ExcelSportsbookPotentialWinLimitQuery message)
        => await ExcelView(message);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> ResetCurrentWinAmount(ResetCurrentWinAmountCommand command)
    {
        var result = await mediator.Send(command);
        return Json(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> ResetCurrentSportsbookWinAmount(ResetCurrentSportsbookWinAmountCommand command)
    {
        var result = await mediator.Send(command);
        return Json(result);
    }
}