using Fanex.Data.Repository;

namespace AlphaPlus.Results.Criteria.Products;

public class GetParlayResultCriteria : CriteriaBase
{
    public long RefNo { get; init; }

    public override string GetSettingKey() => "Age_BetList_SelMatchResultParlay";

    public override bool IsValid() => RefNo > 0;
}
