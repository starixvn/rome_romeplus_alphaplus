namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

using AlphaPlus.CashService.Site.Features.Shared.Requests;
using Newtonsoft.Json;

public class AddPromotionRequest : BaseActionRequest
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; init; }
    public int PromotionType { get; init; }
    public int StatusId { get; set; } = 1;
    public bool Enabled { get; init; }
    public string ProductSettingsJson { get; set; }

    public List<PromotionLanguageSetting> Contents { get; set; }

    public List<PromotionProductSetting> ProductSettings { get; set; }

    public bool IsValid(out string errMsg)
    {
        errMsg = string.Empty;
        if (FromDate is null || ToDate is null || FromDate >= ToDate)
        {
            errMsg = "Promotion period is invalid.";
            return false;
        }

        if (Contents == null || !Contents.Any())
        {
            errMsg = "At least one content is required.";
            return false;
        }

        var enContent = Contents.FirstOrDefault(c => c.Language.Equals("en", StringComparison.CurrentCultureIgnoreCase));
        if (enContent is null || string.IsNullOrEmpty(enContent.Title) || string.IsNullOrEmpty(enContent.Content) || enContent.Banner is null || enContent.Banner.Length == 0)
        {
            errMsg = "English content is required.";
            return false;
        }
        const int bannerSizeLimit = 20 * 1024 * 1024;

        if (Contents.Any(content => content.Banner.Length > bannerSizeLimit))
        {
            errMsg = "Banner size maximum is 20MB";
            return false;
        }

        return true;
    }
}

public class PromotionProductSetting
{
    [JsonProperty("ProductCategoryID")]
    public int ProductCategoryId { get; set; }

    [JsonIgnore]
    public string ProductCategoryName { get; set; }

    public double Percentage { get; set; }

    [JsonProperty("MinTO")]
    public decimal MinTurnover { get; set; }

    public decimal MaxPayout { get; set; }

    public bool Unlimited { get; set; }

    public List<int> ProductIds { get; set; }
}

public class PromotionLanguageSetting
{
    public string Language { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string BannerName { get; set; }
    public IFormFile Banner { get; set; }
}