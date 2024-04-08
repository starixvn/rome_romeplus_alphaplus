namespace AlphaPlus.Features.Site.Messages.Handlers
{
    using AlphaPlus.Features.Messages.API.Constants;
    using AlphaPlus.Features.Messages.API.Params.Announcement;
    using AlphaPlus.Features.Messages.API.Services;
    using MediatR;
    using Requests.Announcement;
    using ViewModels;

    public class AnnouncementHandler :
        IRequestHandler<AnnouncementsHistoryQuery, AnnouncementHistoryViewModel>
    {
        private readonly AnnouncementService announcementService;
        private readonly AnnouncementFormatService formatAnnouncementService;
        private readonly NewMessageService newMessageService;

        public AnnouncementHandler(
            AnnouncementService announcementService,
            AnnouncementFormatService formatService,
            NewMessageService newMessageService)
        {
            this.announcementService = announcementService;
            this.formatAnnouncementService = formatService;
            this.newMessageService = newMessageService;
        }

        public async Task<AnnouncementHistoryViewModel> Handle(AnnouncementsHistoryQuery message, CancellationToken cancellation)
        {
            var parameters = (AnnouncementsHistoryParams)message.ConvertToParameters();

            var announcements = announcementService.GetAnnouncementsHistory(parameters);
            var formatedAnnouncements = formatAnnouncementService.Format(announcements, parameters);

            var unreadMessage = newMessageService.GetNumberOfUnreadMessage(parameters);

            return new AnnouncementHistoryViewModel(
                formatedAnnouncements,
                parameters.FromDate,
                parameters.ToDate,
                MessageType.System,
                message.RoleId,
                unreadMessage);
        }
    }
}
