namespace AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.ViewModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class LimitTypeSelectionViewModel
    {
        public LimitTypeSelectionViewModel(IEnumerable<SelectListItem> limitType, int userSelectedId)
        {
            LimitType = limitType;
            UserSelectedLimitTypeId = userSelectedId;
        }

        public IEnumerable<SelectListItem> LimitType { get; }

        public int UserSelectedLimitTypeId { get; }
    }
}