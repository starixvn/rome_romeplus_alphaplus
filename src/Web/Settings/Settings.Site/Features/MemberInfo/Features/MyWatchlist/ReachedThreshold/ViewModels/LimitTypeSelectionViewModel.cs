using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.ViewModels
{
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