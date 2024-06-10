using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

public class GetMatchInfoByIdCriteria(int matchId) : CriteriaBase
{
    public int MatchId { get; } = matchId;

    public override string GetSettingKey() => "Age_Monitor_MatchSelByID";

    public override bool IsValid() => true;
}