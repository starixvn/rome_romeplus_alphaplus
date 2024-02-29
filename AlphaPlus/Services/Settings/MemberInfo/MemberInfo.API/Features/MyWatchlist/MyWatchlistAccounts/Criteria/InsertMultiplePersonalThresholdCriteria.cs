namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria
{
    using Fanex.Data.Repository;

    public class InsertMultiplePersonalThresholdCriteria : CriteriaBase
    {
        public InsertMultiplePersonalThresholdCriteria(
            string usernames, 
            int agentId, 
            decimal? winningThreshold, 
            decimal? losingThreshold,
            int userId,
            int subAccId, 
            string ip,
            int sporttype = 0)
        {
            Usernames = usernames;
            AgentId = agentId;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            UserId = userId;
            SubAccId = subAccId;
            IP = ip;
            Sporttype = sporttype;
        }

        public string Usernames { get; }

        public int AgentId { get; }

        public decimal? WinningThreshold { get; }

        public decimal? LosingThreshold { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public int Sporttype { get; }

        public override string GetSettingKey() => "Acc_WLMonitored_Personal_Multiple_Ins";

        public override bool IsValid() => AgentId > 0 && !string.IsNullOrEmpty(Usernames);
    }
}