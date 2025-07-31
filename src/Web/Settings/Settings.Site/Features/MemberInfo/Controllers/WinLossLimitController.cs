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

    public Task<ActionResult> SportsbookPotentialWinLimit(SportsbookPotentialWinLimitQuery message) {
        var tokens = antiforgery.GetAndStoreTokens(HttpContext);
        ViewData["RequestVerificationToken"] = tokens.RequestToken;	
        return View(message, "~/Features/MemberInfo/Views/WinLossLimit/SportsbookPotentialWinLimit.cshtml");
    }

    public async Task<ActionResult> PotentialExcel(ExcelSportsbookPotentialWinLimitQuery message)
        => await ExcelView(message);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> ResetCurrentWinAmount(ResetCurrentWinAmountCommand command)
    {
        if(!userSession.IsAgent || userSession.IsInternal)
        {
            return Json(new { isSuccessful = false, message = "Unauthorized" });
        }

        command.SubAccId = viewer.SubAccId;
        command.SubAccName = viewer.SubAccName;
        command.UserId = viewer.Id;
        command.IP = viewer.IP;
        command.SettingStatus = AppSettings.SettingStatus;
        command.Language = viewer.Language;
        var result = await mediator.Send(command);
        return Json(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> ResetCurrentSportsbookWinAmount(ResetCurrentSportsbookWinAmountCommand command)
    {
        if(!userSession.IsAgent || userSession.IsInternal)
        {
            return Json(new { isSuccessful = false, message = "Unauthorized" });
        }
        
        command.SubAccId = viewer.SubAccId;
        command.SubAccName = viewer.SubAccName;
        command.UserId = viewer.Id;
        command.IP = viewer.IP;
        command.SettingStatus = AppSettings.SettingStatus;
        command.SiteDB = (short)userSession.SiteDBGroupId;
        command.Language = viewer.Language;
        var result = await mediator.Send(command);
        return Json(result);
    }
}