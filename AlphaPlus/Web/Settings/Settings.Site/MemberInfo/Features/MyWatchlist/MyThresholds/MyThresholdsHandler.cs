namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds.Requests;
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds.ViewModels;
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyThresholds;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyThresholds.Params;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class MyThresholdsHandler :
        IRequestHandler<MyThresholdIndexPageQuery, MyThresholdIndexPageViewModel>,
        IRequestHandler<MyThresholdHistoryQuery, IEnumerable<ReachHistoryModel>>,
        IRequestHandler<MyThresholdQuery, ThresholdViewModel>,
        IRequestHandler<MyThresholdCommand, Result>
    {
        private readonly IBrowserHelper browserHelper;
        private readonly MyThresholdsService myThresholdsService;

        public MyThresholdsHandler(
            IBrowserHelper browserHelper,
            MyThresholdsService myThresholdsService)
        {
            this.browserHelper = browserHelper;
            this.myThresholdsService = myThresholdsService;
        }

        public async Task<MyThresholdIndexPageViewModel> Handle(MyThresholdIndexPageQuery message, CancellationToken cancellation)
        {
            var isMobile = browserHelper.IsMobile(isOnlyTablet: false);
            var canUpdateAccount = message.CanUpdateAccount;
            var myThresholdTask = myThresholdsService.GetMyThreshold(message.ViewerId, message.Site);
            var myThresholdHistoryTask = Handle((MyThresholdHistoryQuery)message, cancellation);

            await Task.WhenAll(myThresholdTask, myThresholdHistoryTask);

            return new MyThresholdIndexPageViewModel(
                message,
                isMobile,
                canUpdateAccount,
                await myThresholdTask,
                await myThresholdHistoryTask);
        }

        public Task<IEnumerable<ReachHistoryModel>> Handle(MyThresholdHistoryQuery message, CancellationToken cancellation)
        {
            var param = new SelectMyThresholdHistoryParam(
                message.ViewerId,
                message.FromDate,
                message.ToDate,
                message.SortType,
                message.IsDesc,
                message.LimitedDateToShowInfo,
                message.Site);

            return myThresholdsService.GetMyThresholdHistory(param);
        }

        public async Task<ThresholdViewModel> Handle(MyThresholdQuery message, CancellationToken cancellation)
        {
            var myThreshold = await myThresholdsService.GetMyThreshold(message.ViewerId, message.Site);

            return myThreshold == null
                ? new ThresholdViewModel(null, null)
                : new ThresholdViewModel(myThreshold.WinningThreshold, myThreshold.LosingThreshold);
        }

        public Task<Result> Handle(MyThresholdCommand message, CancellationToken cancellation)
            => myThresholdsService.UpdateMyThreshold(new UpdateMyThresholdParam(
                message.ViewerId,
                message.WinningThreshold,
                message.LosingThreshold,
                message.ViewerId,
                message.SubAccId,
                message.IP,
                message.Site));
    }
}