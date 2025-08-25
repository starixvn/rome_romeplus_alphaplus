using Newtonsoft.Json;

namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionProductSetting : BasePromotion
{
    [JsonProperty("ProductCategoryID")]
    public int ProductCategoryId { get; set; }

    [JsonIgnore]
    public string ProductCategoryName { get; set; }

    public double Percentage { get; set; }

    public decimal MinTurnover { get; set; }

    public decimal MaxPayout { get; set; }

    [JsonIgnore]
    public bool Unlimited => MaxPayout == 0 && !string.IsNullOrEmpty(ProductList);

    public string Product => ProductIds is { Count: > 0 } ? string.Join(",", ProductIds) : string.Empty;

    [JsonIgnore]
    public string ProductList { get; set; }

    [JsonIgnore]
    public List<int> ProductIds { get; set; }

    [JsonIgnore]
    public List<Product> Products { get; set; }
}