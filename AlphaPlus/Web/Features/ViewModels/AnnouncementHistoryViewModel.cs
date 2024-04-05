namespace AlphaPlus.Features.Site.Messages.ViewModels
{
    using System;
    using System.Collections.Generic;
    using AlphaPlus.Features.Messages.Models.Announcement;
    using AlphaPlus.Features.Messages.Models.Message;

    public class AnnouncementHistoryViewModel : MessageBase
    {
        public AnnouncementHistoryViewModel(
            IEnumerable<AlphaAnnouncement> announcements,
            DateTime fromDate,
            DateTime toDate,
            int messageType,
            byte roleId,
            NumberOfUnreadMessage unreadMessage) : base(messageType, roleId, fromDate, toDate, unreadMessage)
        {
            Announcements = announcements;
            MessageType = Features.Messages.Constants.MessageType.System;
        }

        public IEnumerable<AlphaAnnouncement> Announcements { get; }

        public DateTime MinDate => new DateTime(2017, 7, 1);

        public DateTime MaxDate => DateTime.Now;

        public DateTime CurrentDate => DateTime.Now;
    }
}