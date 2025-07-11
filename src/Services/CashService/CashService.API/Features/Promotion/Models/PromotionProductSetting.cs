using Newtonsoft.Json;

namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionProductSetting : BasePromotion
{
    [JsonProperty("ProductCategoryID")]
    public int ProductCategoryId { get; set; }

    [JsonIgnore]
    public string ProductCategoryName { get; set; }
    public double Percentage { get; set; }

    [JsonProperty("MinTO")]
    public decimal MinTurnover { get; set; }
    public decimal MaxPayout { get; set; }

    public string Product => ProductIds != null && ProductIds.Count > 0 ? string.Join(",", ProductIds) : string.Empty;

    [JsonIgnore]
    public string ProductList { get; set; }

    [JsonIgnore]
    public List<int> ProductIds { get; set; }


    [JsonIgnore]
    public List<Product> Products { get; set; }
}