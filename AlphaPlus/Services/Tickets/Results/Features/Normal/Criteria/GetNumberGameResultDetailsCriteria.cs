using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetNumberGameResultDetailsCriteria(int matchId) : CriteriaBase
{
    public int MatchId { get; } = matchId;

    public override string GetSettingKey() => "Age_BetList_SelMatchResultNumberGame";

    public override bool IsValid() => MatchId > 0;
}