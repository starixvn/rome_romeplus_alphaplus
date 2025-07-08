namespace AlphaPlus.CashService.Site.Features.Promotion.Models.PromotionItems;

public class LanguageSetting
{
    public string LangCode { get; set; }
    public string Title { get; set; }
    public string Banner { get; set; }
    public string Content { get; set; }
    public bool IsMainLanguage => LangCode.Equals("en", StringComparison.CurrentCultureIgnoreCase);
}