using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts;
using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.ReachedThreshold;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Params;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.ViewModels;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold
{
    public class ReachedThresholdHandler :
        IRequestHandler<ReachedThresholdQuery, ReachedThresholdViewModel>,
        IRequestHandler<ReachlistAccountsQuery, ReachedThresholdPagingViewModel>,
        IRequestHandler<ReachHistoryQuery, ReachHistoryViewModel>,
        IRequestHandler<ReachHistoryListQuery, IEnumerable<ReachHistoryModel>>
    {
        private readonly ReachedThresholdService reachedThresholdService;
        private readonly MyWatchlistAccountsService myWatchlistAccountsService;
        private readonly IBrowserHelper browserHelper;

        public ReachedThresholdHandler(
            ReachedThresholdService reachedThresholdService,
            MyWatchlistAccountsService myWatchlistAccountsService,
            IBrowserHelper browserHelper)
        {
            this.reachedThresholdService = reachedThresholdService;
            this.myWatchlistAccountsService = myWatchlistAccountsService;
            this.browserHelper = browserHelper;
        }

        public async Task<ReachedThresholdViewModel> Handle(ReachedThresholdQuery message, CancellationToken cancellation)
        {
            var isMobile = browserHelper.IsMobile(isOnlyTablet: false);
            var query = new ReachlistAccountsQuery()
            {
                Username = message.Username,
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                Status = message.Status,
                LimitType = message.LimitType,
                SortType = message.SortType,
                IsDesc = message.IsDesc,
                PageIndex = message.PageIndex,
                PageSize = message.PageSize
            };

            var pagingViewModel = await Handle(query, cancellation);

            var param = new SelectGlobalThresholdParam(message.CustId, message.Site);
            var globalThreshold = await myWatchlistAccountsService.GetGlobalThreshold(param);

            return new ReachedThresholdViewModel(message, globalThreshold, isMobile, pagingViewModel);
        }

        public async Task<ReachedThresholdPagingViewModel> Handle(ReachlistAccountsQuery message, CancellationToken cancellation)
        {
            var param = new SelectReachListParam(
                message.CustId,
                message.Username,
                message.FromDate,
                message.ToDate,
                message.Status != 0 ? ReachedThresholdStatusConstants.GetReachedThresholdStatus(message.Status) : string.Empty,
                message.LimitType,
                message.SortType,
                message.IsDesc,
                message.PageSize,
                message.PageIndex,
                message.LimitedDateToShowInfo,
                message.Site);

            var reachlistAccountReport = await reachedThresholdService.GetReachlistAccounts(param);

            return new ReachedThresholdPagingViewModel(reachlistAccountReport, message);
        }

        public async Task<ReachHistoryViewModel> Handle(ReachHistoryQuery message, CancellationToken cancellation)
        {
            var query = new ReachHistoryListQuery()
            {
                SelectedCustId = message.SelectedCustId,
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                LimitType = message.LimitType,
                SortType = message.SortType,
                IsDesc = message.IsDesc,
            };

            var reachHistories = await Handle(query, cancellation);

            return new ReachHistoryViewModel(message, reachHistories);
        }

        public async Task<IEnumerable<ReachHistoryModel>> Handle(ReachHistoryListQuery message, CancellationToken cancellation)
        {
            var param = new SelectReachDetailParam(
             message.SelectedCustId,
             message.FromDate,
             message.ToDate,
             message.LimitType,
             message.SortType,
             message.IsDesc,
             message.LimitedDateToShowInfo,
             message.Site);

            return await reachedThresholdService.GetReachDetail(param);
        }
    }
}