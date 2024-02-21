using Alpha.Main.Site.Features.Dashboard.BalanceInfo;
using AlphaPlus.Core.Models;
using Fanex.Data.Repository;

namespace Alpha.Main.Site.Features.Dashboard.Statistics
{
    public interface IStatisticsService
    {
        Task<Statistics> GetStatistics(int custId, UserLevel userLevel);

        Task<IEnumerable<TopWinningMember>> GetTopWinningMembers(int custId, UserLevel userLevel);

        Task<IEnumerable<TopTurnoverMember>> GetTopTurnoverMembers(int custId, UserLevel userLevel);

        Task<IEnumerable<NewMember>> GetNewMembers(int custId);
    }

    public class StatisticsService(IBalanceInfoService balanceInfoService, IDynamicRepository dynamicRepository) : IStatisticsService
    {
        private readonly IBalanceInfoService balanceInfoService = balanceInfoService;
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public Task<Statistics> GetStatistics(int custId, UserLevel userLevel)
            => dynamicRepository.GetAsync<Statistics>(new GetStatisticsCriteria(userLevel, custId));

        public async Task<IEnumerable<TopWinningMember>> GetTopWinningMembers(int custId, UserLevel userLevel)
        {
            var balanceUpDayInfo = await balanceInfoService.GetBalanceUpDayInfo();
            var criteria = new GetTopWinningMembersCriteria(
                userLevel,
                custId,
                balanceUpDayInfo.BalanceUpDay,
                balanceUpDayInfo.MoveBetsDay);

            return await dynamicRepository.FetchAsync<TopWinningMember>(criteria);
        }

        public async Task<IEnumerable<TopTurnoverMember>> GetTopTurnoverMembers(int custId, UserLevel userLevel)
        {
            var balanceUpDayInfo = await balanceInfoService.GetBalanceUpDayInfo();
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