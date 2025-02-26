using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers
{
    public class WinLossLimitController : SiteBaseController
    {
        public WinLossLimitController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(WinLossLimitQuery message)
            => await View(message, "~/Features/MemberInfo/Views/WinLossLimit/Index.cshtml");
    }
}