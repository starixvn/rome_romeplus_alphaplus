using AlphaPlus.Features.Messages.API.Models;
using AlphaPlus.Features.Messages.API.Constants;
using AlphaPlus.Features.Messages.API.Criteria.Message;
using Fanex.Data.Repository;
using AlphaPlus.Features.Messages.API.Models.Message;
using AlphaPlus.Features.Messages.API.Params;

namespace AlphaPlus.Features.Messages.API.Services
{
    public interface MessageHistoryService
    {
        Task<PagedReportData<MessageHistory>> GetMessagesHistory(MessageHistoryParams messageParams);
    }

    public class MessageHistoryServiceImpl : MessageHistoryService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly AnnouncementService announcementService;
        public MessageHistoryServiceImpl(
            AnnouncementService announcementService,
            IDynamicRepository dynamicRepository)
        {
            this.announcementService = announcementService;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<PagedReportData<MessageHistory>> GetMessagesHistory(MessageHistoryParams messageParams)
        {
            messageParams.StartDate = GetAvailableLastSeenDate(messageParams.StartDate, messageParams.MessageType, messageParams.CustId, messageParams.SubAccId);
            var fromDate = GetLimitedDate(messageParams.StartDate, messageParams.LimitedDate, messageParams.MessageType);

            var criteria = new GetMessagesHistoryCriteria(
                messageParams.CustId,
                messageParams.ParentSites,
                messageParams.SpecialType,
                fromDate,
                messageParams.EndDate,
                messageParams.IsTest,
                messageParams.MessageType,
                messageParams.Site,
                messageParams.PageIndex - 1,
                messageParams.PageSize
                );

            announcementService.UpdateLastSeenAnnouncement(messageParams.CustId, messageParams.MessageType, messageParams.IsAdminUser, messageParams.SubAccId);
            var messsages = await dynamicRepository.FetchAsync<MessageHistory>(criteria);

            return new PagedReportData<MessageHistory>(
                messsages,
                messageParams.PageIndex,
                messageParams.PageSize,
                criteria.TotalRows);
        }

        private DateTime GetAvailableLastSeenDate(DateTime fromDate, int messageType, int custId, int subAccId)
        {
            var lastSeenAnnouncement = announcementService.GetLastSeenAnnouncement(custId, subAccId);
            var lastSeenDate = lastSeenAnnouncement.GetLastSeenByType(messageType);
            if (fromDate == DateTime.MinValue)
            {
                var threeDayAgo = DateTime.Today.AddDays(-3);
                var threeMonthAgo = DateTime.Today.AddMonths(-3);
                if (threeDayAgo < lastSeenDate)
                {
                    return threeDayAgo;
                }
                else if (lastSeenDate < threeDayAgo && lastSeenDate > threeMonthAgo)
                {
                    return lastSeenDate;
                }
                else
                {
                    return threeMonthAgo;
                }
            }

            return fromDate;
        }

        private static DateTime GetLimitedDate(DateTime fromDate, DateTime limitedDate, int messageType)
            => messageType == MessageType.Personal && fromDate < limitedDate
                ? limitedDate
                : fromDate;
    }
}