using AlphaPlus.Features.Messages.API.Models.Message;
using AlphaPlus.Features.Messages.API.Params;
using AlphaPlus.Features.Messages.API.Services;
using MediatR;
using AlphaPlus.Features.Site.Messages.Requests.MessageHistory;
using AlphaPlus.Features.Site.Messages.ViewModels;

namespace AlphaPlus.Features.Site.Messages.Handlers
{
    public class MessageHistoryHandler : IRequestHandler<MessageHistoryQuery, MessagesHistoryViewModel>
    {
        private readonly MessageHistoryService messageService;
        private readonly NewMessageService newMessageService;
        private readonly AnnouncementService announcementService;

        public MessageHistoryHandler(
            MessageHistoryService messageService,
            NewMessageService newMessageService,
            AnnouncementService announcementService)
        {
            this.messageService = messageService;
            this.newMessageService = newMessageService;
            this.announcementService = announcementService;
        }

        public async Task<MessagesHistoryViewModel> Handle(MessageHistoryQuery message, CancellationToken cancellation)
        {
            var lastSeenDate = GetLastSeenDate(message.CustId, message.SubAccId, message.MessageType);
            var messageParams = (MessageHistoryParams)message.ConvertToParameters();
            var messages = messageService.GetMessagesHistory(messageParams);

            var numberOfUnreadMessage = await newMessageService.GetNumberOfUnreadMessage(messageParams);
            UpdateUnreadMessageOnSession(message, numberOfUnreadMessage);

            return new MessagesHistoryViewModel(
                messages,
                lastSeenDate,
                message.Language,
                messageParams.StartDate,
                message.EndDate,
                message.MessageType,
                message.RoleId,
                numberOfUnreadMessage);
        }

        private static void UpdateUnreadMessageOnSession(MessageHistoryQuery message, NumberOfUnreadMessage unreadMessage)
        {
            message.NumberOfUnreadMessage = unreadMessage;
        }

        private DateTime GetLastSeenDate(int custId, int subAccId, int messageType)
        {
            var lastSeenMessage = announcementService.GetLastSeenAnnouncement(custId, subAccId);

            return lastSeenMessage.GetLastSeenByType(messageType);
        }

    }
}