namespace AlphaPlus.CashService.Site.Features.Promotion.Models.PromotionItems;

using Products;

public class Promotion
{
    public int PromotionId { get; set; }
    public int SiteDbGroupId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Period => $"{FromDate:MM/dd/yyyy} - {ToDate:MM/dd/yyyy}";
    public bool Enabled { get; set; }
    public List<ProductCategorySetting> ProductSettings { get; set; }
    public List<LanguageSetting> LanguageSettings { get; set; }
}