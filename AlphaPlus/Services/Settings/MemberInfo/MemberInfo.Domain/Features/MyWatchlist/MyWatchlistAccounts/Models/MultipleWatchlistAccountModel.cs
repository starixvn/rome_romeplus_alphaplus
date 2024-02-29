namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models
{
    using System.Collections.Generic;

    public class MultipleWatchlistAccountModel
    {
        
        public MultipleWatchlistAccountModel(IEnumerable<WatchlistAccountModel> alreadyAddedAccounts, IEnumerable<WatchlistAccountModel> newlyAddedAccounts)
        {
            AlreadyAddedAccounts = alreadyAddedAccounts;
            NewlyAddedAccounts = newlyAddedAccounts;
        }

        public IEnumerable<WatchlistAccountModel> AlreadyAddedAccounts { get; }
        public IEnumerable<WatchlistAccountModel> NewlyAddedAccounts { get; }

    }
}
