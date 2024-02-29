namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class AddPersonalThresholdParam
    {
        public AddPersonalThresholdParam(
            string usernames, 
            int agentId, 
            decimal? winningThreshold, 
            decimal? losingThreshold, 
            int userId, 
            int subAccId, 
            string ip,
            string siteName)
        {
            Usernames = usernames;
            AgentId = agentId;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            UserId = userId;
            SubAccId = subAccId;
            IP = ip;
            SiteName = siteName;
        }

        public string Usernames { get; }

        public int AgentId { get; }

        public decimal? WinningThreshold { get; }

        public decimal? LosingThreshold { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string SiteName { get; }
    }
}
