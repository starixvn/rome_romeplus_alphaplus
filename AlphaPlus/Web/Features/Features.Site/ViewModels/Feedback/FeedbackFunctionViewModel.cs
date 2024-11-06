namespace AlphaPlus.Features.Site.Messages.ViewModels.Feedback
{
    public class FeedbackFunctionViewModel
    {
        public FeedbackFunctionViewModel()
        {
        }

        public FeedbackFunctionViewModel(
            int functionId,
            string functionName,
            IEnumerable<FeedbackAreaViewModel> feedbackAreas)
        {
            FunctionId = functionId;
            FunctionName = functionName;
            FeedbackAreas = feedbackAreas;
        }

        public int FunctionId { get; }

        public string FunctionName { get; }

        public IEnumerable<FeedbackAreaViewModel> FeedbackAreas { get; }
    }
}