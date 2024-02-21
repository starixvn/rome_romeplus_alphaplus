using AlphaPlus.Core.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace Alpha.Main.Site.Features.Dashboard.BalanceInfo
{
    public interface IBalanceInfoService
    {
        Task<BalanceInfo> GetBalanceInfo(int custId, UserLevel userLevel);

        Task<BalanceUpDayInfo> GetBalanceUpDayInfo();
    }

    public class BalanceInfoService(IDynamicRepository dynamicRepository, ICacheService cacheService) : IBalanceInfoService
    {
        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;
        private readonly ICacheService cacheService = cacheService;

        public async Task<BalanceInfo> GetBalanceInfo(int custId, UserLevel userLevel)
        {
            var moveBetsDay = (await GetBalanceUpDayInfo()).MoveBetsDay;
            var criteria = new GetBalanceInfoCriteria(userLevel, custId, moveBetsDay);

            var balanceInfo = await dynamicRepository.GetAsync<BalanceInfo>(criteria);

            return balanceInfo;
        }

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