using AlphaPlus.Core.Models;
using Alpha.Main.Criteria.BalanceInfo;
using Fanex.Data.Repository;
using Alpha.Main.Models.BalanceInfo;

namespace Alpha.Main.Services.Impl
{
    public class BalanceInfoServiceImpl(
        BalanceUpDayInfoService balanceUpDayInfoService,
        IDynamicRepository dynamicRepository) : BalanceInfoService
    {
        private readonly BalanceUpDayInfoService balanceUpDayInfoService = balanceUpDayInfoService;
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public async Task<BalanceInfo> GetBalanceInfo(int custId, UserLevel userLevel)
        {
            var moveBetsDay = (await balanceUpDayInfoService.GetBalanceUpDayInfo()).MoveBetsDay;
            var criteria = new GetBalanceInfoCriteria(userLevel, custId, moveBetsDay);

            var balanceInfo = await dynamicRepository.GetAsync<BalanceInfo>(criteria);

            return balanceInfo;
        }
    }
}