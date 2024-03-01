namespace AlphaPlus.Settings.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

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