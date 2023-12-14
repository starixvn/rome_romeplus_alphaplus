using AlphaPlus.Core.Models;
using Fanex.Data.Repository;
using Alpha.Main.Models.Statistics;

namespace Alpha.Main.DataAccesses
{
    public interface StatisticsDataAccess
    {
        Statistics GetStatistics(ICriteria criteria, UserLevel userLevel);

        List<TopWinningMember> GetTopWinningMembers(ICriteria criteria);

        List<TopTurnoverMember> GetTopTurnoverMembers(ICriteria criteria);
    }
}