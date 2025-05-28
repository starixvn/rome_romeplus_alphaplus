using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using AlphaPlus.Core.Filters;
using AlphaPlus.Settings.Site.Features.MemberInfo.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;

public partial class WinLossLimitController(IMediator mediator) : SiteBaseController(mediator)
{
    public Task<ActionResult> Index(WinLossLimitQuery message)
    {
        return View(message, "~/Features/MemberInfo/Views/WinLossLimit/Index.cshtml");
    }

    public Task<ActionResult> SportsbookPotentialWinLimit(SportsbookPotentialWinLimitQuery message)
    {
        return View(message, "~/Features/MemberInfo/Views/WinLossLimit/SportsbookPotentialWinLimit.cshtml");
    }

    [AllowProtectedMode]
    public async Task<IActionResult> PotentialExcel(ExcelSportsbookPotentialWinLimitQuery message)
    {
        return await ExcelView(message);
    }
}