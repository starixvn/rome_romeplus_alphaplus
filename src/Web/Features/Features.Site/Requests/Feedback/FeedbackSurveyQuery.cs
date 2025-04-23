using AlphaPlus.Features.Site.Messages.ViewModels.Feedback;
using MediatR;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Features.Site.Messages.Requests.Feedback
{
    public class FeedbackSurveyQuery
    {
        public FeedbackSurveyQuery() : this(null) { }

        public FeedbackSurveyQuery(IUserSession userSession)
        {
            userSession = userSession ?? UserSession.Current;
            NickName = userSession.NickName;
            IsInternal = userSession.IsInternal;
        }

        public string NickName { get; set; }
        public bool IsInternal { get; set; }
    }
}