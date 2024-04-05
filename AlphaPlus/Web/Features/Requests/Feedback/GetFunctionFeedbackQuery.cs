namespace AlphaPlus.Features.Site.Messages.Requests.Feedback
{
    using AlphaPlus.Features.Site.Messages.ViewModels.Feedback;
    using MediatR;
    using AlphaPlus.Core.Models;

    public class GetFunctionFeedbackQuery : IRequest<FeedbackFunctionViewModel>
    {
        public GetFunctionFeedbackQuery(IUserSession userSession)
        {
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            Language = userSession.Language;
        }

        public int Id { get; set; }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Language { get; }
    }
}