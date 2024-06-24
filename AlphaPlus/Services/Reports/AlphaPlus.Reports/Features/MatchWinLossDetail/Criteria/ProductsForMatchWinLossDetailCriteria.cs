using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

internal class ProductsForMatchWinLossDetailCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_Rpt_MatchWinLossDetail_GetProductList";

    public override bool IsValid() => true;
}