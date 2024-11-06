using MediatR;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Features.Site.Messages.Requests.Feedback
{
    public class AddFeedbackQuery : IRequest<Result>
    {
        private const int MaxMessageLength = 4000;

        public AddFeedbackQuery() : this(null) { }

        public AddFeedbackQuery(IUserSession userSession)
        {
            userSession = userSession ?? UserSession.Current;
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            IsInternal = userSession.IsInternal;
        }

        public string Message { get; set; }

        public byte Rate { get; set; }

        public int? FunctionId { get; set; }

        public IEnumerable<FeedbackAreaQuery> Areas { get; set; }

        public int CustId { get; }

        public int SubAccId { get; }

        public bool IsInternal { get; }

        public bool IsValid()
            => !IsInternal &&
            (Rate != 0 || (!string.IsNullOrWhiteSpace(Message) && Message.Length <= MaxMessageLength) || Areas?.Any() == true);
    }
}