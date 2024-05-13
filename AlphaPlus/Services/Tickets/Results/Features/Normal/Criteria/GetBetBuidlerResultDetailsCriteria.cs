using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetBetBuidlerResultDetailsCriteria(int matchId, int marketId) : CriteriaBase
{
    public int MatchId { get; } = matchId;

    public int MarketId { get; } = marketId;

    public override string GetSettingKey() => "Age_TicketResult_BetBuilder";

    public override bool IsValid() => MatchId > 0 && MarketId > 0;
}