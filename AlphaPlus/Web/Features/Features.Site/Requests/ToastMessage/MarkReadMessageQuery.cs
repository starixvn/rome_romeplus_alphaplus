namespace AlphaPlus.Features.Site.Messages.Requests.ToastMessage
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class MarkReadMessageQuery : IRequest<Result>
    {
        public MarkReadMessageQuery() : this(null) {}
        
        private readonly IUserSession userSession;

        public MarkReadMessageQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? UserSession.Current;
        }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public int Id { get; set; }
    }
}
