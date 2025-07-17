using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;

public partial class WinLossLimitController(IMediator mediator, IUserSession userSession) : SiteBaseController(mediator)
{
    public Task<ActionResult> Index(WinLossLimitQuery message)
    {
        return View(message, "~/Features/MemberInfo/Views/WinLossLimit/Index.cshtml");
    }

    public Task<ActionResult> SportsbookPotentialWinLimit(SportsbookPotentialWinLimitQuery message)
        => View(message, "~/Features/MemberInfo/Views/WinLossLimit/SportsbookPotentialWinLimit.cshtml");

    public async Task<ActionResult> PotentialExcel(ExcelSportsbookPotentialWinLimitQuery message)
        => await ExcelView(message);
}