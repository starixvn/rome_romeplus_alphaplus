namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionProductSetting : BasePromotion
{
    public int ProductCategoryId { get; set; }
    public double Percentage { get; set; }
    public decimal MinTurnover { get; set; }
    public decimal MaxPayout { get; set; }
    public bool Unlimited { get; set; }
    public string ProductList { get; set; }
    public List<Product> Products { get; set; }
}