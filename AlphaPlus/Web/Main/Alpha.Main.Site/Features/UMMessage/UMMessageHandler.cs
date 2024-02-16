using MediatR;

namespace Alpha.Main.Site.Features.UMMessage
{
    public class UMMessageHandler(IUMMessageService messageService) : IRequestHandler<UMMessageQuery, IEnumerable<UMMessageDTO>>
    {
        private readonly IUMMessageService messageService = messageService;

        public async Task<IEnumerable<UMMessageDTO>> Handle(UMMessageQuery message, CancellationToken cancellationToken)
            => await messageService.GetMessagesAsync(ToUMMessageParam(message));

        private static UMMessageParam ToUMMessageParam(UMMessageQuery message)
            => new(message.CustId, message.Site, message.CurrencyId, message.SuperId);
    }
}