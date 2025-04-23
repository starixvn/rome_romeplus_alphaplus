using AlphaPlus.Features.Messages.API.Models.Announcement;
using AlphaPlus.Features.Messages.API.Params.Announcement;
using AlphaPlus.Features.Messages.API.Services;
using MediatR;
using AlphaPlus.Features.Site.Messages.Requests.Announcement;

namespace AlphaPlus.Features.Site.Messages.Handlers
{
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
            var announcement = await announcementService.GetPopUpAnnouncements(parameters);
            UpdatePopupIsShowedOnSession(message, announcement != null);

            return formatAnnouncementService.Format(announcement, parameters);
        }

        private static void UpdatePopupIsShowedOnSession(PopupAnnouncementQuery message, bool hasAnnouncement)
        {
            message.IsShowedPopup = hasAnnouncement ? hasAnnouncement : message.IsShowedPopup;
        }
    }
}
