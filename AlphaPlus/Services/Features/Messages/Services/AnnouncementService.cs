using Alpha.Announcements.Models;
using Alpha.Announcements.Services;
using Alpha.Announcements.Specs;
using AlphaPlus.Features.Messages.API.Client.Requests;
using AlphaPlus.Features.Messages.API.Configurations;
using AlphaPlus.Features.Messages.API.Constants;
using AlphaPlus.Features.Messages.API.Criteria.Announcement;
using AlphaPlus.Features.Messages.API.Models.Announcement;
using AlphaPlus.Features.Messages.API.Params.Announcement;
using AlphaPlus.Features.Messages.Utils;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Features.Messages.API.Services
{
    public interface AnnouncementService
    {
        Task<int> GetNumberOfNewAnnouncements(BaseAnnouncementsParams announcementParams);

        void UpdateLastSeenAnnouncement(int custId, int messageType, bool isAdminUser, int subAccId);

        Task<IEnumerable<AlphaAnnouncement>> GetAnnouncementsHistory(AnnouncementsHistoryParams announcementParams);

        Task<AlphaAnnouncement> GetPopUpAnnouncements(PopupAnnouncementParams announcementParams);

        LastSeenAnnouncement GetLastSeenAnnouncement(int custId, int subAccId);

        Task<IEnumerable<Announcement>> GetAnnouncements(DateTime fromTime, DateTime toTime);

        IEnumerable<AlphaAnnouncement> GetAnnouncementsByUser(
           IEnumerable<Announcement> announcementToday,
           BaseAnnouncementsParams announcementParams,
           DateTime lastSeenAnnouncement);
    }

    public class AnnouncementServiceImpl : AnnouncementService
    {
        private const int CachedSecondAnnouncement = 30;
        private const string DefaultLanguage = "en-US";

        private readonly AnnouncementClient announcementClient;
        private readonly SpecificationService specificationService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly IMemoryCache cacheService;

        public AnnouncementServiceImpl(
            AnnouncementClient announcementClient,
            SpecificationService specificationService,
            IDynamicRepository dynamicRepository,
            IMemoryCache cacheService)
        {
            this.announcementClient = announcementClient;
            this.specificationService = specificationService;
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public void UpdateLastSeenAnnouncement(int custId, int messageType, bool isAdminUser, int subAccId)
        {
            if (!isAdminUser)
            {
                dynamicRepository.Execute(new UpdateLastSeenAnnouncementCriteria(
                    custId,
                    MessageType.ToString(messageType),
                    subAccId));
            }
        }

        public async Task<int> GetNumberOfNewAnnouncements(BaseAnnouncementsParams announcementParams)
        {
            var lastSeenAnnouncement = GetLastSeenAnnouncement(announcementParams.CustId, announcementParams.SubAccId);

            var fromDate = GetAvailableLastSeenDate(DateTime.MinValue, lastSeenAnnouncement.LastSeenSystem);

            var newAnnouncements = await GetNewAnnouncements(fromDate);

            var announcements = GetAnnouncementsByUser(newAnnouncements, announcementParams, lastSeenAnnouncement.LastSeenSystem);

            return announcements == null || !announcements.Any() ? 0 : announcements.Count(i => i.ReleasedDate >= lastSeenAnnouncement.LastSeenSystem);
        }

        public async Task<IEnumerable<AlphaAnnouncement>> GetAnnouncementsHistory(AnnouncementsHistoryParams announcementParams)
        {
            var lastSeenAnnouncement = GetLastSeenAnnouncement(announcementParams.CustId, announcementParams.SubAccId);

            announcementParams.FromDate = GetAvailableLastSeenDate(announcementParams.FromDate, lastSeenAnnouncement.LastSeenSystem);

            var announcementsHistory = await GetAnnouncementsHistory(announcementParams.FromDate, announcementParams.ToDate);
            var announcements = GetAnnouncementsByUser(announcementsHistory, announcementParams, lastSeenAnnouncement.LastSeenSystem);

            UpdateLastSeenAnnouncement(announcementParams.CustId, MessageType.System, announcementParams.IsAdminUser, announcementParams.SubAccId);

            return announcements == null ? [] : announcements.OrderByDescending(i => i.ReleasedDate);
        }

        public async Task<AlphaAnnouncement> GetPopUpAnnouncements(PopupAnnouncementParams announcementParams)
        {
            if (announcementParams.IsShowedPopup)
            {
                return null;
            }

            var announcementsHistory = await GetAvailablePopUpAnnouncements();
            var announcements = GetAnnouncementsByUser(announcementsHistory, announcementParams, DateTime.Now);

            return announcements.OrderByDescending(i => i.ReleasedDate).FirstOrDefault();
        }

        public LastSeenAnnouncement GetLastSeenAnnouncement(int custId, int subAccId)
        {
            var lastSeen = dynamicRepository.Get<LastSeenAnnouncement>(new GetLastSeenAnnouncementCriteria(custId, subAccId));

            return lastSeen ?? new LastSeenAnnouncement(custId, SystemTime.Now, SystemTime.Now, SystemTime.Now, SystemTime.Now);
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncements(DateTime fromTime, DateTime toTime)
            => await announcementClient.GetAnnouncementsHistory(new AnnouncementHistoryRequest(fromTime, toTime, AppSettings.Environment, string.Empty));

        private static DateTime GetAvailableLastSeenDate(DateTime fromDate, DateTime lastSeenDate)
        {
            if (fromDate == DateTime.MinValue)
            {
                var threeDayAgo = DateTime.Today.AddDays(-3);
                var sixMonthAgo = DateTime.Today.AddMonths(-6);
                if (threeDayAgo < lastSeenDate)
                {
                    return threeDayAgo;
                }
                else if (lastSeenDate < threeDayAgo && lastSeenDate > sixMonthAgo)
                {
                    return lastSeenDate;
                }
                else
                {
                    return sixMonthAgo;
                }
            }

            return fromDate;
        }

        private async Task<IEnumerable<Announcement>> GetNewAnnouncements(DateTime fromDate)
        {
            var newAnnouncements = await cacheService.GetOrCreateAsync(
                 $"AlphaPlus.Features.Messages.Announcement_{fromDate}_{AppSettings.SiteLabel}",
                 cacheEntry =>
                 {
                     cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedSecondAnnouncement));
                     return GetAnnouncementsHistory(fromDate, DateTime.Now);
                 });

            return newAnnouncements;
        }

        private async Task<IEnumerable<Announcement>> GetAvailablePopUpAnnouncements()
        {
            var request = new AnnouncementHistoryRequest(DateTime.Today.AddDays(-30), DateTime.Now, AppSettings.Environment, AppSettings.SiteLabel);
            var availablePopupAnnouncements = await cacheService.GetOrCreateAsync(
                $"AlphaPlus.Features.Messages.PopupAnnouncement_{AppSettings.SiteLabel}",
                async cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedSecondAnnouncement));
                    return (await announcementClient.GetAnnouncementsHistory(request)).Where(i => DateTime.Now <= i.ExpiredDate && i.IsPopup);
                });

            return availablePopupAnnouncements;
        }

        private async Task<IEnumerable<Announcement>> GetAnnouncementsHistory(DateTime fromDate, DateTime toDate)
        {
            var request = new AnnouncementHistoryRequest(fromDate, toDate, AppSettings.Environment, AppSettings.SiteLabel);

            return (await announcementClient.GetAnnouncementsHistory(request))
                .Where(i => i.ReleasedDate <= DateTime.Now)
                .OrderBy(i => i.ReleasedDate);
        }

        public IEnumerable<AlphaAnnouncement> GetAnnouncementsByUser(
            IEnumerable<Announcement> announcementToday,
            BaseAnnouncementsParams announcementParams,
            DateTime lastSeenAnnouncement)
        {
            foreach (var announcement in announcementToday)
            {
                if (announcement.Specifications == null
                    || !announcement.Specifications.Any()
                    || announcement.Specifications.Any(s => s == null)
                    || specificationService.IsSatisfied(announcement.Specifications, announcementParams.SpecificationParamter))
                {
                    var messageContent = GetMessageContentByLanguage(announcement.Contents, announcementParams.Language);
                    yield return new AlphaAnnouncement(
                        announcement.Id,
                        messageContent,
                        announcement.Code,
                        announcement.ReleasedDate,
                        lastSeenAnnouncement,
                        announcement.Specifications ?? Enumerable.Empty<ISpecification<IParam>>());
                }
            }
        }

        private static string GetMessageContentByLanguage(IEnumerable<MessageContent> messages, string languageKey)
        {
            var message = messages == null ? default : messages?.FirstOrDefault(i => i.LanguageKey.Equals(languageKey) && !string.IsNullOrEmpty(i.Text));

            message = message ?? messages?.FirstOrDefault(i => i.LanguageKey.Equals(DefaultLanguage) && !string.IsNullOrEmpty(i.Text));

            return message == null ? string.Empty : message.Text;
        }
    }
}