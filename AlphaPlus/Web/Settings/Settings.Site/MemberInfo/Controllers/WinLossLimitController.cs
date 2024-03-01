namespace Alpha.MemberInfo.Site.Controllers
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

        public ActionResult Index(WinLossLimitQuery message)
            => View(message, "~/Views/WinLossLimit/Index.cshtml");
    }
}