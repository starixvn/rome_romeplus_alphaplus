namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models
{
    public class ReachlistAccountReport
    {
        public ReachlistAccountReport(IEnumerable<ReachlistAccountModel> reachlistAccounts, int totalItems)
        {
            ReachlistAccounts = reachlistAccounts;
            TotalItems = totalItems;
        }

        public IEnumerable<ReachlistAccountModel> ReachlistAccounts { get; }

        public int TotalItems { get; }
    }
}
