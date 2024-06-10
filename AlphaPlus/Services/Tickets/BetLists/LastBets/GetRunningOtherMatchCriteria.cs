using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

public class GetRunningOtherMatchCriteria : CriteriaBase
{
    public int MatchId { get; } = 0;

    public override string GetSettingKey() => "Age_Monitor_MatchSel";

    public override bool IsValid() => true;
}
