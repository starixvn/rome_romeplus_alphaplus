using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class SelectWatchlistSummaryParam
    {
        public SelectWatchlistSummaryParam(int agentId, string siteName)
            : this(agentId, 0, DateTime.Today, DateTime.Today, ReachedThresholdLimitTypeConstants.All, siteName)
        {
        }

        public SelectWatchlistSummaryParam(
            int agentId,
            int sportType,
            DateTime fromDate,
            DateTime toDate,
            byte limitType,
            string siteName)
        {
            AgentId = agentId;
            SportType = sportType;
            FromDate = fromDate;
            ToDate = toDate;
            LimitType = limitType;
            SiteName = siteName;
        }

        public int AgentId { get; }

        public int SportType { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte LimitType { get; }

        public string SiteName { get; }
    }
}
