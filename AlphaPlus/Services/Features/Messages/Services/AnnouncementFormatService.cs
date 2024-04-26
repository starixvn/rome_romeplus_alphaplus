namespace AlphaPlus.Features.Messages.API.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Core.Helpers;
    using Constants;
    using Models.Announcement;
    using Params.Announcement;

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