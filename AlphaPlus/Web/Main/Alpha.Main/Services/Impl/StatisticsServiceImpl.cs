using AlphaPlus.Core.Models;
using Alpha.Main.Criteria.Statistics;
using Fanex.Data.Repository;
using Alpha.Main.Models.Statistics;

namespace Alpha.Main.Services.Impl
{
    public class StatisticsServiceImpl(
        BalanceUpDayInfoService balanceUpDayInfoService,
        IDynamicRepository dynamicRepository) : StatisticsService
    {
        private readonly BalanceUpDayInfoService balanceUpDayInfoService = balanceUpDayInfoService;
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public Task<Statistics> GetStatistics(int custId, UserLevel userLevel)
            => dynamicRepository.GetAsync<Statistics>(new GetStatisticsCriteria(userLevel, custId));

        public async Task<IEnumerable<TopWinningMember>> GetTopWinningMembers(int custId, UserLevel userLevel)
        {
            var balanceUpDayInfo = await balanceUpDayInfoService.GetBalanceUpDayInfo();
            var criteria = new GetTopWinningMembersCriteria(
                userLevel,
                custId,
                balanceUpDayInfo.BalanceUpDay,
                balanceUpDayInfo.MoveBetsDay);

            return await dynamicRepository.FetchAsync<TopWinningMember>(criteria);
        }

        public async Task<IEnumerable<TopTurnoverMember>> GetTopTurnoverMembers(int custId, UserLevel userLevel)
        {
            var balanceUpDayInfo = await balanceUpDayInfoService.GetBalanceUpDayInfo();
            var criteria = new GetTopTurnoverMembersCriteria(
                userLevel,
                custId,
                balanceUpDayInfo.BalanceUpDay,
                balanceUpDayInfo.MoveBetsDay);

            return await dynamicRepository.FetchAsync<TopTurnoverMember>(criteria);
        }

        public Task<IEnumerable<NewMember>> GetNewMembers(int custId)
            => dynamicRepository.FetchAsync<NewMember>(new GetNewMembersCriteria(custId));
    }
}