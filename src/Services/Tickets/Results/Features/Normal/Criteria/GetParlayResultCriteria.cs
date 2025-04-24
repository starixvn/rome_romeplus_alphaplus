using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetParlayResultCriteria(long refNo) : CriteriaBase
{
    public long RefNo { get; } = refNo;

    public override string GetSettingKey() => "Age_BetList_SelMatchResultParlay";

    public override bool IsValid() => RefNo > 0;
}
