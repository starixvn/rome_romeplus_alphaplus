namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

using Fanex.Data.Repository;

public class GetProductListCriteria : CriteriaBase
{
    public string Site { get; set; } = string.Empty;
    public int Currency { get; set; }
    public int SettingStatus { get; set; }

    public override string GetSettingKey() => "Age_CashService_PromotionProductList";

    public override bool IsValid() => true;
}