using Newtonsoft.Json;

namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionLanguageSetting : BasePromotion
{
    [JsonProperty("language")]
    public string Language { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("content")]
    public string Content { get; set; }
    
    [JsonProperty("banner")]
    public string Banner { get; set; }
}