namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

using Fanex.Data.Repository;

public class GetProductListCriteria(string site, int currency, int settingStatus) : CriteriaBase
{
    public string Site { get; set; } = site;
    public int Currency { get; set; } = currency;
    public int SettingStatus { get; set; } = settingStatus;

    public override string GetSettingKey() => "Age_CashService_PromotionProductList";

    public override bool IsValid() => true;
}