namespace AlphaPlus.Features.Site.Messages.Handlers
{
    using System.Collections.Generic;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Features.Messages.Models.ToastMessage;
    using AlphaPlus.Features.Messages.Params.ToastMessage;
    using AlphaPlus.Features.Messages.Services;
    using AlphaPlus.Features.Site.Messages.Requests.ToastMessage;
    using MediatR;

    public class ToastMessageHandler :
        IRequestHandler<ToastMessageQuery, IEnumerable<ToastMessageModel>>,
        IRequestHandler<MarkReadMessageQuery, Result>
    {
        private readonly ToastMessageService service;

        public ToastMessageHandler(ToastMessageService service)
        {
            this.service = service;
        }

        public async Task<IEnumerable<ToastMessageModel>> Handle(ToastMessageQuery message, CancellationToken cancellation)
        {
            var messageParams = new GetToastMessagesParams
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                Language = message.Language,
                LastToastId = message.LastToastId
            };

            var messages = service.Get(messageParams);

            return messages;
        }

        public async Task<Result> Handle(MarkReadMessageQuery message, CancellationToken cancellation)
        {
            var messageParams = new MarkReadMessageParams
            {
                Id = message.Id,
                TargetId = message.CustId,
                SubAccId = message.SubAccId
            };

            return service.MarkRead(messageParams);
        }
    }
}
