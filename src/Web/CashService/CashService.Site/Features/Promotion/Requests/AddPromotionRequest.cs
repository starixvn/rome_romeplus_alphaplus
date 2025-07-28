using System.Data.SqlTypes;
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

public class AddPromotionRequest : BaseActionRequest
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; init; }
    public int PromotionType { get; init; }
    public int StatusId { get; set; } = 1; // Default status
    public bool Enabled { get; init; }    
    public string ProductSettingsJson { get; set; }

    public List<PromotionLanguageSetting> Contents { get; set; }
    
    public List<PromotionProductSetting> ProductSettings { get; set; }
   
    
    public bool IsValid()
    {
        return SqlDateTime.MinValue.Value < FromDate && FromDate <= ToDate;
    }
}

// Add these classes to match the API models
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