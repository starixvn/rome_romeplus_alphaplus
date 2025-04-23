using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Products;

internal class GetProductsDetailCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_Rpt_GetProductDetail";

    public override bool IsValid() => true;
}