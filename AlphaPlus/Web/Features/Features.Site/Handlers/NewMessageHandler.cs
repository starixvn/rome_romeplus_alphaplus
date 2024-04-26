namespace AlphaPlus.Features.Site.Messages.Handlers
{
    using AlphaPlus.Features.Messages.API.Models.Message;
    using AlphaPlus.Features.Messages.API.Params.Announcement;
    using AlphaPlus.Features.Messages.API.Services;
    using MediatR;
    using Requests.NewMessage;

    public class NewMessageHandler :
         IRequestHandler<NumberOfNewMessageQuery, NumberOfUnreadMessage>
    {
        private readonly NewMessageService messageService;
        public NewMessageHandler(
            NewMessageService messageService)
        {
            this.messageService = messageService;
        }

        public async Task<NumberOfUnreadMessage> Handle(NumberOfNewMessageQuery message, CancellationToken cancellation)
        {
            // messageService.MoveNewMessage(message.CustId, message.UserName, message.RoleId, message.IsAdminUser);

            var parameters = (NumberOfNewMessageParams)message.ConvertToParameters();

            var unreadMessage = await messageService.CheckNewUnreadMessage(parameters);

            UpdateUnreadMessageSession(message, unreadMessage);

            return unreadMessage;
        }

        private static void UpdateUnreadMessageSession(NumberOfNewMessageQuery message, NumberOfUnreadMessage unreadMessage)
        {
            message.UnreadMessage = unreadMessage;
        }
    }
}
