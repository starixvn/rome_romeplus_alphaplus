namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class DeletePersonalPresetParam
    {
        public DeletePersonalPresetParam(int id, int agentId, string siteName)
        {
            ID = id;
            AgentId = agentId;
            SiteName = siteName;
        }

        public int ID { get; }

        public int AgentId { get; }

        public string SiteName { get; }
    }
}
