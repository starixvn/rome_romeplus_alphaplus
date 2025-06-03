using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLossDetail.Criteria;

public class WinLossDetailExtraProductCriteria : CriteriaBase
{
    public WinLossDetailExtraProductCriteria(int custId)
    {
        CustId = custId;
    }

    public int CustId { get; set; }

    public override string GetSettingKey() => "Age_GetOfferedProductByCust_Extra";

    public override bool IsValid() => true;
}
