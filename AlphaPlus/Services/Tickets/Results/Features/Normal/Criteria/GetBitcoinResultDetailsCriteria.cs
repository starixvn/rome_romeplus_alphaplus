using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetBitcoinResultDetailsCriteria(int matchId) : CriteriaBase
{
    public int MatchId { get; } = matchId;

    public override string GetSettingKey() => "Acc_BetList_Match3rdResultSel";

    public override bool IsValid() => MatchId > 0;
}
