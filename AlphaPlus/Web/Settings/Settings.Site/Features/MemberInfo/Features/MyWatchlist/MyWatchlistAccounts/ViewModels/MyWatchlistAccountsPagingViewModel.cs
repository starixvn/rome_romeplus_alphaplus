namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using System.Collections.Generic;

    public class MyWatchlistAccountsPagingViewModel
    {
        public MyWatchlistAccountsPagingViewModel(WatchlistAccountReport watchlistAccountReport, WatchlistAccountsQuery request)
        {
            WatchlistAccounts = watchlistAccountReport.WatchlistAccounts;
            TotalItems = watchlistAccountReport.TotalItems;
        }

        public IEnumerable<WatchlistAccountModel> WatchlistAccounts { get; }

        public int TotalItems { get; }
    }
}