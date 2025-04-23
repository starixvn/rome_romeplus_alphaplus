using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

internal class GetMonitoredWatchlistMembersCriteria(int agentId) : CriteriaBase
{
    public int AgentId { get; } = agentId;

    public bool ExportAll { get; } = true;

    public override string GetSettingKey() => "Acc_WLMonitored_Player_WatchList_Sel";

    public override bool IsValid() => AgentId > 0;
}
