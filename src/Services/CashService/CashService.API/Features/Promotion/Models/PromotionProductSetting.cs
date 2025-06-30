namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionProductSetting
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public double Percentage { get; set; }
    public decimal MinTurnover { get; set; }
    public decimal MaxPayout { get; set; }
    public bool Unlimited { get; set; }
}