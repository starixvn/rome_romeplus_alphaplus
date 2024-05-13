using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetMetricGamingResultCriteria(long betId) : CriteriaBase
{
    public long BetId { get; } = betId;

    public override string GetSettingKey()
        => "All_BetList_MatchResultDetail3rdSel";

    public override bool IsValid() => BetId > 0;
}