namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;

    public class ManageGlobalThresholdViewModel
    {
        public ManageGlobalThresholdViewModel(GlobalThresholdModel globalThreshold)
        {
            Threshold = new ThresholdViewModel(globalThreshold?.WinningThreshold, globalThreshold?.LosingThreshold);
        }

        public ThresholdViewModel Threshold { get; }
    }
}