namespace AlphaPlus.Features.Site.Messages.ViewModels.Feedback
{
    public class CheckExistsFeedbackViewModel
    {
        public CheckExistsFeedbackViewModel(bool isExist, string functionName)
        {
            IsExist = isExist;
            FunctionName = functionName;
        }

        public bool IsExist { get; }

        public string FunctionName { get; }
    }
}