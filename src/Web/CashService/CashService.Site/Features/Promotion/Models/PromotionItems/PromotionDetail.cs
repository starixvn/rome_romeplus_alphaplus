namespace AlphaPlus.CashService.Site.Features.Promotion.Models.PromotionItems;

using AlphaPlus.Shared.AppResources.CashServices;
using Enum;
using Site.Helpers;

public class PromotionDetail
{
    public int PromotionId { get; set; }
    public string PromotionTitle { get; set; }
    public int PromotionTypeId { get; set; }
    public string PromotionTypeName => EnumHelper.GetEnumDescriptionByNumber<PromotionType>(PromotionTypeId, CashServicesResx.ResourceManager);
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool Enabled { get; set; }
    public List<ProductCategorySetting> ProductSettings { get; set; }
    public List<LanguageSetting> LanguageSettings { get; set; }
}