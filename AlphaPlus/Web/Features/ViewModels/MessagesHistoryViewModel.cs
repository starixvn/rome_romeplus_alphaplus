namespace AlphaPlus.Features.Site.Messages.ViewModels
{
    using System;
    using AlphaPlus.Features.Messages.Models;
    using AlphaPlus.Features.Messages.Models.Message;
    using AlphaPlus.Features.Site.Messages.ViewModels.Components;

    public class MessagesHistoryViewModel : MessageBase
    {
        public MessagesHistoryViewModel(
            PagedReportData<MessageHistory> reportData,
            DateTime lastSeenAnnouncement,
            string languageKey,
            DateTime startDate,
            DateTime endDate,
            int messageType,
            byte roleId,
            NumberOfUnreadMessage unreadMessage) : base(messageType, roleId, startDate, endDate, unreadMessage)
        {
            ReportData = reportData;
            LastSeenAnnouncement = lastSeenAnnouncement;
            LanguageKey = languageKey;
        }

        public DateTime LastSeenAnnouncement { get; }

        public string LanguageKey { get; }

        public bool IsNewMessage(DateTime postTime) => postTime > LastSeenAnnouncement;
        public int BuildNo(int no)
            => no + 1 + ((ReportData.PageIndex - 1) * ReportData.PageSize);

        public PagerViewModel PagerViewModel =>
                  new PagerViewModel(ReportData.PageIndex, ReportData.PageSize, ReportData.TotalRecords);

        public PagedReportData<MessageHistory> ReportData { get; }

    }

}
