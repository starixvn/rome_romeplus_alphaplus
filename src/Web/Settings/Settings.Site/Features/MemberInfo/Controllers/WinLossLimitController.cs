using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using AlphaPlus.Core.Filters;
using AlphaPlus.Settings.Site.Features.MemberInfo.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;

public partial class WinLossLimitController(IMediator mediator, IUserSession userSession) : SiteBaseController(mediator)
{
    public Task<ActionResult> Index(WinLossLimitQuery message)
    {
        return View(message, "~/Features/MemberInfo/Views/WinLossLimit/Index.cshtml");
    }

    public Task<ActionResult> SportsbookPotentialWinLimit(SportsbookPotentialWinLimitQuery message)
    {
        if (!AppSettings.IsSupportPotentialWinLimit(userSession.Site))
        {
            return Task.FromResult<ActionResult>(Unauthorized());
        }

        return View(message, "~/Features/MemberInfo/Views/WinLossLimit/SportsbookPotentialWinLimit.cshtml");
    }

    public async Task<ActionResult> PotentialExcel(ExcelSportsbookPotentialWinLimitQuery message)
    {   
        if (!AppSettings.IsSupportPotentialWinLimit(userSession.Site))
        {
            return Unauthorized();
        }

        return await ExcelView(message);
    }
}