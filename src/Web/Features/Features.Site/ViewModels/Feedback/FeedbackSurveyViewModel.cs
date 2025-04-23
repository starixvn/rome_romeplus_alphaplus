using AlphaPlus.Features.Site.Messages.Requests.Feedback;

namespace AlphaPlus.Features.Site.Messages.ViewModels.Feedback
{
    public class FeedbackSurveyViewModel
    {
        public FeedbackSurveyViewModel()
        {
        }

        public FeedbackSurveyViewModel(FeedbackSurveyQuery query)
        {
            NickName = query.NickName;
            IsInternal = query.IsInternal;
        }

        public string NickName { get; }
        public bool IsInternal { get; }
    }
}