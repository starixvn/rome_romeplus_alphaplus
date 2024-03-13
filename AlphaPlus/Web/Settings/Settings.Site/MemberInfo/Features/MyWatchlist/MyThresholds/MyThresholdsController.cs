namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds
{
    using System.Threading.Tasks;
    using AlphaPlus.Settings.Site.Controllers;
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds.Requests;
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds.ViewModels;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class MyThresholdsController : SiteBaseController
    {
        private const string IndexPath = "~/MemberInfo/Features/MyWatchlist/MyThresholds/Views/Index.cshtml";
        private const string SettingsPath = "~/MemberInfo/Features/MyWatchlist/MyThresholds/Views/Settings.cshtml";

        public MyThresholdsController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(MyThresholdIndexPageQuery message)
            => await ViewAsync<MyThresholdIndexPageViewModel>(message, IndexPath);

        [HttpPost]
        public async Task<ActionResult> GetHistory(MyThresholdHistoryQuery message)
            => await JsonAsync(message);

        public async Task<ActionResult> Settings(MyThresholdQuery message)
            => await ViewAsync(message, SettingsPath);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateSettings(MyThresholdCommand message)
            => await JsonAsync(message);
    }
}