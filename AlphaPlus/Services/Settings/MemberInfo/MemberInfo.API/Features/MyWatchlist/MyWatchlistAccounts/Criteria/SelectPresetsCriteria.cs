namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria
{
    using Fanex.Data.Repository;

    public class SelectPresetsCriteria : CriteriaBase
    {
        public SelectPresetsCriteria(int agentId, int sporttype = 0)
        {
            AgentId = agentId;
            Sporttype = sporttype;
        }

        public int AgentId { get; }

        public int Sporttype { get; }

        public override string GetSettingKey() => "Acc_WLMonitored_Preset_Sel";

        public override bool IsValid() => AgentId > 0;
    }
}
