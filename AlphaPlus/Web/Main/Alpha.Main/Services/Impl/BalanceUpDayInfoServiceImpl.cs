using Alpha.Main.Criteria.BalanceInfo;
using Fanex.Caching;
using Fanex.Data.Repository;
using Alpha.Main.Models.BalanceInfo;

namespace Alpha.Main.Services.Impl
{
    public class BalanceUpDayInfoServiceImpl(IDynamicRepository dynamicRepository, ICacheService cacheService) : BalanceUpDayInfoService
    {
        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;
        private readonly ICacheService cacheService = cacheService;

        public async Task<BalanceUpDayInfo> GetBalanceUpDayInfo()
        {
            var balanceUpDayInfo = await cacheService.GetAsync<BalanceUpDayInfo>(BalanceUpDayKey);

            if (balanceUpDayInfo == null)
            {
                var criteria = new GetBalanceUpDayInfoCriteria();

                balanceUpDayInfo = await dynamicRepository.GetAsync<BalanceUpDayInfo>(criteria);

                cacheService.Set(BalanceUpDayKey, balanceUpDayInfo, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
            }

            return balanceUpDayInfo;
        }
    }
}