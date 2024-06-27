namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests;
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ReachedThresholdController : SiteBaseController
    {
        private const string IndexPath = "~/Features/MemberInfo/Features/MyWatchlist/ReachedThreshold/Views/Index.cshtml";
        private const string ReachHistoryPath = "~/Features/MemberInfo/Features/MyWatchlist/ReachedThreshold/Views/ReachHistory.cshtml";

        public ReachedThresholdController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(ReachedThresholdQuery message)
            => await ViewAsync(message, IndexPath);

        public async Task<ActionResult> ReachHistoryIndex(ReachHistoryQuery message)
            => await ViewAsync(message, ReachHistoryPath);

        [HttpPost]
        public async Task<ActionResult> GetReachlistThreshold(ReachlistAccountsQuery message)
        {
            if (!message.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized));
            }

            return Json(await mediator.Send(message));
        }

        [HttpPost]
        public async Task<ActionResult> GetReachHistoryList(ReachHistoryListQuery message)
        {
            if (!message.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized));
            }

            return Json(await mediator.Send(message));
        }

        public async Task<ActionResult> Excel(ReachedThresholdExcelQuery message)
            => await ExcelViewAsync(message);
    }
}