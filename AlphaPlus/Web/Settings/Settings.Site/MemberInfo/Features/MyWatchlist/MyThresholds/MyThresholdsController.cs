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
        private const string IndexPath = "~/Features/MyWatchlist/MyThresholds/Views/Index.cshtml";
        private const string SettingsPath = "~/Features/MyWatchlist/MyThresholds/Views/Settings.cshtml";

        public MyThresholdsController(IMediator mediator)
            : base(mediator)
        {
        }

        public Task<ActionResult> Index(MyThresholdIndexPageQuery message)
            => ViewAsync<MyThresholdIndexPageViewModel>(message, IndexPath);

        [HttpPost]
        public Task<ActionResult> GetHistory(MyThresholdHistoryQuery message)
            => JsonAsync(message);

        public Task<ActionResult> Settings(MyThresholdQuery message)
            => ViewAsync(message, SettingsPath);

        [HttpPost, ValidateAntiForgeryToken]
        public Task<ActionResult> UpdateSettings(MyThresholdCommand message)
            => JsonAsync(message);
    }
}