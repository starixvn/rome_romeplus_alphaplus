namespace AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.ViewModels
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
    using System.Collections.Generic;

    public class ReachedThresholdPagingViewModel
    {
        public ReachedThresholdPagingViewModel(ReachlistAccountReport reachlistAccountReport, ReachlistAccountsQuery request)
        {
            ReachlistAccounts = reachlistAccountReport.ReachlistAccounts;
            TotalItems = reachlistAccountReport.TotalItems;
        }

        public IEnumerable<ReachlistAccountModel> ReachlistAccounts { get; }

        public int TotalItems { get; }
    }
}