namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PresetSelectionViewModel
    {
        public PresetSelectionViewModel(IEnumerable<SelectListItem> presets, int userSelectedId)
        {
            Presets = presets;
            UserSelectedStatusId = userSelectedId;
        }

        public IEnumerable<SelectListItem> Presets { get; }

        public int UserSelectedStatusId { get; }
    }
}