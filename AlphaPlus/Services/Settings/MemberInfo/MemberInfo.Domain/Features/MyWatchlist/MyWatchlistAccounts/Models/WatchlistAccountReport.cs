namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models
{
    public class WatchlistAccountReport
    {
        public WatchlistAccountReport(IEnumerable<WatchlistAccountModel> watchlistAccounts, int totalItems)
        {
            WatchlistAccounts = watchlistAccounts;
            TotalItems = totalItems;
        }

        public IEnumerable<WatchlistAccountModel> WatchlistAccounts { get; }

        public int TotalItems { get; }
    }
}
