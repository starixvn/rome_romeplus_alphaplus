namespace Alpha.MemberInfo.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    public class ThresholdViewModel
    {
        public ThresholdViewModel(decimal? winningThreshold, decimal? losingThreshold)
        {
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
        }

        public decimal? WinningThreshold { get; }

        public decimal? LosingThreshold { get; }
    }
}