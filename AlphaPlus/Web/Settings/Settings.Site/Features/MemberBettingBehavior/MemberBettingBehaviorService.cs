using AlphaPlus.PartialUM.Services;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic;
using AlphaPlus.Settings.Site.Features.MemberBettingBehavior.Profile;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior
{
    public interface IMemberBettingBehaviorService
    {
        Task<CustomerProfile> GetCustomerProfile(int custId, string siteName);

        Task<MemberBettingStatistic> GetMemberBettingBehaviorStatistics(int custId, DateTime startDate, DateTime endDate, string siteName, string language);

        Task<BettingStatisticExportData> GetMemberBettingStatisticExcelData(int custId, DateTime startDate, DateTime endDate, string siteName, string language);
    }

    public class MemberBettingBehaviorService : IMemberBettingBehaviorService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;
        private readonly CacheRefService refService;
        private readonly IPartialUMService partialUMService;
        private readonly HttpClient httpClient;

        public MemberBettingBehaviorService(
            IDynamicRepository dynamicRepository,
            ICacheService cacheService,
            IPartialUMService partialUMService,
            CacheRefService refService,
            HttpClient httpClient)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
            this.partialUMService = partialUMService;
            this.refService = refService;
            this.httpClient = httpClient;
        }


        public Task<CustomerProfile> GetCustomerProfile(int custId, string siteName)
        {
            throw new NotImplementedException();
        }

        public Task<MemberBettingStatistic> GetMemberBettingBehaviorStatistics(int custId, DateTime startDate, DateTime endDate, string siteName, string language)
        {
            throw new NotImplementedException();
        }

        public Task<BettingStatisticExportData> GetMemberBettingStatisticExcelData(int custId, DateTime startDate, DateTime endDate, string siteName, string language)
        {
            throw new NotImplementedException();
        }
    }
}