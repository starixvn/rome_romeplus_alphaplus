namespace AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.ViewModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class StatusSelectionViewModel
    {
        public StatusSelectionViewModel(IEnumerable<SelectListItem> statuses, int userSelectedId)
        {
            Statuses = statuses;
            UserSelectedStatusId = userSelectedId;
        }

        public IEnumerable<SelectListItem> Statuses { get; }

        public int UserSelectedStatusId { get; }
    }
}