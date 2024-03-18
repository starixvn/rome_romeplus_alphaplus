namespace AlphaPlus.Settings.Site.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Requests.WinLossLimit;

    public class WinLossLimitController : SiteBaseController
    {
        public WinLossLimitController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(WinLossLimitQuery message)
            => await View(message, "~/MemberInfo/Views/WinLossLimit/Index.cshtml");
    }
}