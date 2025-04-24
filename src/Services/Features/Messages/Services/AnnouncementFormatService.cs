using AlphaPlus.Core.Helpers;
using AlphaPlus.Features.Messages.API.Constants;
using AlphaPlus.Features.Messages.API.Models.Announcement;
using AlphaPlus.Features.Messages.API.Params.Announcement;

namespace AlphaPlus.Features.Messages.API.Services
{
    public interface AnnouncementFormatService
    {
        IEnumerable<AlphaAnnouncement?> Format(IEnumerable<AlphaAnnouncement> announcements, BaseAnnouncementsParams message);
        AlphaAnnouncement? Format(AlphaAnnouncement announcement, BaseAnnouncementsParams message);
    }

    public class AnnouncementFormatServiceImpl : AnnouncementFormatService
    {
        public AlphaAnnouncement? Format(AlphaAnnouncement announcement, BaseAnnouncementsParams message)
        {
            if (announcement != null)
            {
                announcement.Content = ReplaceMessageSiteTag(announcement.Content, message.IsMobileDevice);
            }

            return announcement;
        }

        public IEnumerable<AlphaAnnouncement?> Format(IEnumerable<AlphaAnnouncement> announcements, BaseAnnouncementsParams message)
            => announcements.Select(i => Format(i, message));

        private static string ReplaceMessageSiteTag(string content, bool isMoblie)
            => content.Replace(MessageConstants.SiteMessageTag, isMoblie ? MicroSites.FeatureUrl("~") : MicroSites.FeatureUrl());
    }
}