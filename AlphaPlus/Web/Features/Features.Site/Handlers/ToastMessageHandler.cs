using AlphaPlus.Core.Models;
using AlphaPlus.Features.Messages.API.Models.ToastMessage;
using AlphaPlus.Features.Messages.API.Params.ToastMessage;
using AlphaPlus.Features.Messages.API.Services;
using AlphaPlus.Features.Site.Messages.Requests.ToastMessage;
using MediatR;

namespace AlphaPlus.Features.Site.Messages.Handlers
{
    public class ToastMessageHandler(ToastMessageService service) :
        IRequestHandler<ToastMessageQuery, IEnumerable<ToastMessageModel>>,
        IRequestHandler<MarkReadMessageQuery, Result>
    {
        private readonly ToastMessageService service = service;

        public async Task<IEnumerable<ToastMessageModel>> Handle(ToastMessageQuery message, CancellationToken cancellation)
        {
            var messageParams = new GetToastMessagesParams
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                Language = message.Language,
                LastToastId = message.LastToastId
            };

            var messages = await service.Get(messageParams);

            return messages;
        }

        public Task<Result> Handle(MarkReadMessageQuery message, CancellationToken cancellation)
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
