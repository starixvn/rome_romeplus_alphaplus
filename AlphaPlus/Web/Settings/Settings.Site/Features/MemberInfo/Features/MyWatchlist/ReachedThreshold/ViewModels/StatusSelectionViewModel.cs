using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.ViewModels
{
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