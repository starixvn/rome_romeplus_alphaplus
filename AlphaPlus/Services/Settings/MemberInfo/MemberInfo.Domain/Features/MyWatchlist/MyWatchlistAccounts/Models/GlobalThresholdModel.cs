namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models
{
    public class GlobalThresholdModel
    {
        public GlobalThresholdModel()
        {
        }

        public GlobalThresholdModel(decimal? winningThreshold, decimal? losingThreshold)
        {
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
        }

        public decimal? WinningThreshold { get; set; }

        public decimal? LosingThreshold { get; set; }
    }
}
