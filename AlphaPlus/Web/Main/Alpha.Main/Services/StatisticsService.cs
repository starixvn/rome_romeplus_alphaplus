using AlphaPlus.Core.Models;
using Alpha.Main.Models.Statistics;

namespace Alpha.Main.Services
{
    public interface StatisticsService
    {
        Task<Statistics> GetStatistics(int custId, UserLevel userLevel);

        Task<IEnumerable<TopWinningMember>> GetTopWinningMembers(int custId, UserLevel userLevel);

        Task<IEnumerable<TopTurnoverMember>> GetTopTurnoverMembers(int custId, UserLevel userLevel);

        Task<IEnumerable<NewMember>> GetNewMembers(int custId);
    }
}