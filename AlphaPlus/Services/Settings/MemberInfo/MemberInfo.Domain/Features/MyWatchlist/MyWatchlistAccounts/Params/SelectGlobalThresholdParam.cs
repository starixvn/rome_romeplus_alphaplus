namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class SelectGlobalThresholdParam
    {
        public SelectGlobalThresholdParam(int agentId, string siteName)
        {
            AgentId = agentId;
            SiteName = siteName;
        }

        public int AgentId { get; }

        public string SiteName { get; }
    }
}
