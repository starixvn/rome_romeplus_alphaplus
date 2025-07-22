namespace AlphaPlus.CashService.Site.Features.Promotion.Models.Products;

public class ProductCategorySetting
{
    public int ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; }
    public double Percentage { get; set; }
    public decimal MaxPayout { get; set; }
    public decimal MinTO { get; set; }
    public bool Unlimited { get; set; }
}