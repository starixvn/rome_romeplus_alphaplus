namespace AlphaPlus.Features.Site.Messages.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Features.Messages.Services;
    using AlphaPlus.Features.Messages.Site.Requests.PortalMessage;
    using MediatR;
    using ViewModels;

    public class PortalMessageHandler :
        IRequestHandler<PortalMessageQuery, PortalMessagesViewModel>,
        IRequestHandler<MarkMessageAsReadQuery, bool>
    {        
        private readonly PortalMessageService noticeService;

        public PortalMessageHandler(PortalMessageService noticeService)
        {
            this.noticeService = noticeService;
        }

        public async Task<PortalMessagesViewModel> Handle(PortalMessageQuery message, CancellationToken cancellation)
        {
            var noticeMessages = noticeService.GetPortalMessages(
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
            var isSuccess = noticeService.MarkMessageAsRead(message.CustId, message.MessageId, message.IsInternal);

            return isSuccess;
        }

    }
}