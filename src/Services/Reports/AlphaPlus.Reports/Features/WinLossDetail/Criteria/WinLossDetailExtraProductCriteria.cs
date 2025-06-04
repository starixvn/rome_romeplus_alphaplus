using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLossDetail.Criteria;

public class WinLossDetailExtraProductCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Age_GetOfferedProductByCust_Extra";

    public override bool IsValid() => CustId > 0;
}
