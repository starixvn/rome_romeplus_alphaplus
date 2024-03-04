using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;

namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    public class ManagePersonalThresholdPresetViewModel
    {
        public ManagePersonalThresholdPresetViewModel(IEnumerable<PersonalPresetModel> personalPresets)
        {
            PersonalPresets = personalPresets;
        }

        public IEnumerable<PersonalPresetModel> PersonalPresets { get; }
    }
}