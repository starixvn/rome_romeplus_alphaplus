namespace AlphaPlus.Features.Site.Messages.Handlers
{
    using AlphaPlus.Features.Messages.Models.Announcement;
    using AlphaPlus.Features.Messages.Params.Announcement;
    using AlphaPlus.Features.Messages.Services;
    using MediatR;
    using Requests.Announcement;

    public class PopupAnnouncementHandler :
        IRequestHandler<PopupAnnouncementQuery, AlphaAnnouncement>
    {
        private readonly AnnouncementService announcementService;
        private readonly AnnouncementFormatService formatAnnouncementService;

        public PopupAnnouncementHandler(
            AnnouncementService announcementService,
            AnnouncementFormatService formatService)
        {
            this.announcementService = announcementService;
            this.formatAnnouncementService = formatService;
        }

        public async Task<AlphaAnnouncement> Handle(PopupAnnouncementQuery message, CancellationToken cancellation)
        {
            var parameters = (PopupAnnouncementParams)message.ConvertToParameters();
            var announcement = announcementService.GetPopUpAnnouncements(parameters);
            UpdatePopupIsShowedOnSession(message, announcement != null);

            return formatAnnouncementService.Format(announcement, parameters);
        }

        private static void UpdatePopupIsShowedOnSession(PopupAnnouncementQuery message, bool hasAnnouncement)
        {
            message.IsShowedPopup = hasAnnouncement ? hasAnnouncement : message.IsShowedPopup;
        }
    }
}
