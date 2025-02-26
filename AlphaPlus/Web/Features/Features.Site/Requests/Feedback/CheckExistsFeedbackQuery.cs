using AlphaPlus.Features.Site.Messages.ViewModels.Feedback;
using MediatR;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Features.Site.Messages.Requests.Feedback
{
    public class CheckExistsFeedbackQuery : GetFunctionFeedbackQuery, IRequest<CheckExistsFeedbackViewModel>
    {
        public CheckExistsFeedbackQuery() : this(null) { }

        public CheckExistsFeedbackQuery(IUserSession userSession) : base(userSession)
        {
        }
    }
}