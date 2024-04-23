namespace AlphaPlus.Features.Site.Messages.Requests.Feedback
{
    using AlphaPlus.Features.Site.Messages.ViewModels.Feedback;
    using MediatR;
    using AlphaPlus.Core.Models;

    public class CheckExistsFeedbackQuery : GetFunctionFeedbackQuery, IRequest<CheckExistsFeedbackViewModel>
    {
        public CheckExistsFeedbackQuery() : this(null) {}
        
        public CheckExistsFeedbackQuery(IUserSession userSession) : base(userSession)
        {
        }
    }
}