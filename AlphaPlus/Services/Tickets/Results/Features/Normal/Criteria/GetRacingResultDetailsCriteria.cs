using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetRacingResultDetailsCriteria(int matchId) : CriteriaBase
{
    public int MatchId { get; } = matchId;

    public override string GetSettingKey() => "Age_BetList_SelMatchResult2";

    public override bool IsValid() => MatchId > 0;
}