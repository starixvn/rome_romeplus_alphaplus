using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetParlay3rdResultCriteria(long refNo) : CriteriaBase
{
    public long RefNo { get; } = refNo;
    public override string GetSettingKey() => "Age_BetList_SelMatch3rdResultParlay";

    public override bool IsValid() => RefNo >= 0;

}