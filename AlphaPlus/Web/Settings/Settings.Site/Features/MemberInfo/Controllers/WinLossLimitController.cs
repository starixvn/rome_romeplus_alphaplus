using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;

public class WinLossLimitController(IMediator mediator) : SiteBaseController(mediator)
{
    public Task<ActionResult> Index(WinLossLimitQuery message)
        => View(message, "~/Features/MemberInfo/Views/WinLossLimit/Index.cshtml");
}