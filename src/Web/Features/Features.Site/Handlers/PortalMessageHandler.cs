using AlphaPlus.Features.Messages.API.Services;
using AlphaPlus.Features.Messages.Site.Requests.PortalMessage;
using MediatR;
using AlphaPlus.Features.Site.Messages.ViewModels;

namespace AlphaPlus.Features.Site.Messages.Handlers
{
    public class PortalMessageHandler(PortalMessageService noticeService) :
        IRequestHandler<PortalMessageQuery, PortalMessagesViewModel>,
        IRequestHandler<MarkMessageAsReadQuery, bool>
    {
        private readonly PortalMessageService noticeService = noticeService;

        public async Task<PortalMessagesViewModel> Handle(PortalMessageQuery message, CancellationToken cancellation)
        {
            var noticeMessages = await noticeService.GetPortalMessages(
                                    message.CustId,
                                    message.ParentSites,
                                    message.TotalMessage,
                                    message.IsTest);

            var noticeMessageWrappers = new List<PortalMessageWrapper>(
                noticeMessages.Select(m => new PortalMessageWrapper(m, message.CurrentLang)));

            var viewModel = new PortalMessagesViewModel(noticeMessageWrappers);

            return viewModel;
        }

        public async Task<bool> Handle(MarkMessageAsReadQuery message, CancellationToken cancellation)
        {
            var isSuccess = await noticeService.MarkMessageAsRead(message.CustId, message.MessageId, message.IsInternal);

            return isSuccess;
        }

    }
}