using Alpha.Main.Models.UMMessage;
using Alpha.Main.Params.UMMessage;
using Alpha.Main.Services;
using MediatR;

namespace Alpha.Main.Site.Features.UMMessage
{
    public class UMMessageHandler(UMMessageService messageService) : IRequestHandler<UMMessageQuery, IEnumerable<UMMessageDTO>>
    {
        private readonly UMMessageService messageService = messageService;

        public async Task<IEnumerable<UMMessageDTO>> Handle(UMMessageQuery message, CancellationToken cancellationToken)
            => await messageService.GetMessagesAsync(ToUMMessageParam(message));

        private static UMMessageParam ToUMMessageParam(UMMessageQuery message)
            => new(message.CustId, message.Site, message.CurrencyId, message.SuperId);
    }
}