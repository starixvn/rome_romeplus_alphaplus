using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    public class PresetSelectionViewModel(IEnumerable<SelectListItem> presets, int userSelectedId)
    {
        public IEnumerable<SelectListItem> Presets { get; } = presets;

        public int UserSelectedStatusId { get; } = userSelectedId;
    }
}