using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    public class MyWatchlistAccountsPagingViewModel
    {
        public MyWatchlistAccountsPagingViewModel(
            WatchlistAccountReport watchlistAccountReport,
            WatchlistSummaryModel watchlistSummary)
        {
            WatchlistAccounts = watchlistAccountReport.WatchlistAccounts;
            TotalItems = watchlistAccountReport.TotalItems;
            WatchlistSummary = watchlistSummary;
        }

        public IEnumerable<WatchlistAccountModel> WatchlistAccounts { get; }

        public int TotalItems { get; }

        public WatchlistSummaryModel WatchlistSummary { get; }
    }
}