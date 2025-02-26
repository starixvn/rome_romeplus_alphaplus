using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components
{
    public class ReviewedStatusSelectionViewModel
    {
        public ReviewedStatusSelectionViewModel(IEnumerable<SelectListItem> statuses, int userSelectedId)
        {
            Statuses = statuses;
            UserSelectedReviewedStatusId = userSelectedId;
        }

        public IEnumerable<SelectListItem> Statuses { get; }

        public int UserSelectedReviewedStatusId { get; }
    }
}