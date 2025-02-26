namespace AlphaPlus.Features.Site.Messages.ViewModels.Feedback
{
    public class FeedbackAreaViewModel
    {
        public FeedbackAreaViewModel()
        {
        }

        public FeedbackAreaViewModel(int areaId, string areaName)
        {
            AreaId = areaId;
            AreaName = areaName;
        }

        public int AreaId { get; }

        public string AreaName { get; }
    }
}