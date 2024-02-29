namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria
{
    using Fanex.Data.Repository;

    public class SelectGlobalThresholdCriteria : CriteriaBase
    {
        public SelectGlobalThresholdCriteria(int agentId, int sporttype = 0)
        {
            AgentId = agentId;
            Sporttype = sporttype;
        }

        public int AgentId { get; }

        public int Sporttype { get; }

        public override string GetSettingKey() => "Acc_WLMonitored_Global_Sel";

        public override bool IsValid() => AgentId > 0;
    }
}
