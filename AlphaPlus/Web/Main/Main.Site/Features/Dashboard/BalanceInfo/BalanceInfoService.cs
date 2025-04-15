using AlphaPlus.Core.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo
{
    public interface IBalanceInfoService
    {
        Task<BalanceInfo> GetBalanceInfo(int custId, UserLevel userLevel);

        Task<BalanceUpDayInfo> GetBalanceUpDayInfo();
    }

    public class BalanceInfoService(IDynamicRepository dynamicRepository, IMemoryCache cacheService) : IBalanceInfoService
    {
        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;
        private readonly IMemoryCache cacheService = cacheService;

        public Task<BalanceInfo> GetBalanceInfo(int custId, UserLevel userLevel)
            => dynamicRepository.GetAsync<BalanceInfo>(new GetBalanceInfoCriteria(userLevel, custId));

        public Task<BalanceUpDayInfo> GetBalanceUpDayInfo()
            => cacheService.GetOrCreateAsync(BalanceUpDayKey, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
                    var criteria = new GetBalanceUpDayInfoCriteria();

                    return dynamicRepository.GetAsync<BalanceUpDayInfo>(criteria);
                });
    }
}