namespace AlphaPlus.Features.Site.Messages.Requests.ToastMessage
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class MarkReadMessageQuery : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public MarkReadMessageQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public int Id { get; set; }
    }
}
