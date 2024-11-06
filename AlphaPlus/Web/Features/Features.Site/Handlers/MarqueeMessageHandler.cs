using AlphaPlus.Features.Messages.API.Models.Message;
using AlphaPlus.Features.Messages.API.Services;
using MediatR;
using AlphaPlus.Features.Site.Messages.Requests.MarqueeMessage;
using AlphaPlus.Features.Site.Messages.ViewModels;
using AlphaPlus.Features.Messages.API.Configurations;

namespace AlphaPlus.Features.Site.Messages.Handlers
{
    public class MarqueeMessageHandler :
        IRequestHandler<MarqueeMessageQuery, MarqueeMessageViewModel>
    {
        private readonly MarqueeMessageService messageService;
        private readonly NewMessageService newMessageService;

        public MarqueeMessageHandler(MarqueeMessageService messageService, NewMessageService newMessageService)
        {
            this.messageService = messageService;
            this.newMessageService = newMessageService;
        }

        public async Task<MarqueeMessageViewModel> Handle(MarqueeMessageQuery message, CancellationToken cancellation)
        {
            await newMessageService.MoveNewMessage(message.CustId, message.UserName, message.RoleId, message.IsAdminUser);

            GeneralMarqueeMessage publicMessage;
            GeneralMarqueeMessage privateMessage;

            if (AppSettings.DisableMessages)
            {
                publicMessage = new GeneralMarqueeMessage();
                privateMessage = new GeneralMarqueeMessage();
            }
            else
            {
                publicMessage = messageService.GetLatestPublicMessage(message.RoleId, message.CurrentLang, message.IsTest, message.CustomerSite);
                privateMessage = messageService.GetLatestPrivateMessage(message.CustId, message.CurrentLang, message.IsTest);
            }

            return new MarqueeMessageViewModel(publicMessage, privateMessage);
        }

    }
}
