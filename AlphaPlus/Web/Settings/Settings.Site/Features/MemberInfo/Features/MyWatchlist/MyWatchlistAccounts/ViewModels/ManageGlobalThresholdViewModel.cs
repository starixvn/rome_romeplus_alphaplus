using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    public class ManageGlobalThresholdViewModel
    {
        public ManageGlobalThresholdViewModel(GlobalThresholdModel globalThreshold)
        {
            Threshold = new ThresholdViewModel(globalThreshold?.WinningThreshold, globalThreshold?.LosingThreshold);
        }

        public ThresholdViewModel Threshold { get; }
    }
}