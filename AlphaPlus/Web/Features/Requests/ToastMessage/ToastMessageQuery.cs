namespace AlphaPlus.Features.Site.Messages.Requests.ToastMessage
{
    using System.Collections.Generic;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Features.Messages.Models.ToastMessage;
    using MediatR;

    public class ToastMessageQuery : IRequest<IEnumerable<ToastMessageModel>>
    {
        private readonly IUserSession userSession;

        public ToastMessageQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public string Language => userSession.LangKey;

        public int LastToastId { get; set; }
    }
}
