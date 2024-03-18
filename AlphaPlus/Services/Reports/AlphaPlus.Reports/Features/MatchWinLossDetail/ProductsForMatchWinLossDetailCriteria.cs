using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public class ProductsForMatchWinLossDetailCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_Rpt_MatchWinLossDetail_GetProductList";

    public override bool IsValid() => true;
}