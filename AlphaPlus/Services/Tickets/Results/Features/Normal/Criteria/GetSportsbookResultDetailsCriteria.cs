using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetSportsbookResultDetailsCriteria(int matchId) : CriteriaBase
{
    public int MatchId { get; } = matchId;

    public override string GetSettingKey() => "Age_Result_MatchSelResultDetail";

    public override bool IsValid() => MatchId >= 0;
}