namespace AlphaPlus.CashService.Site.Features.Promotion.Models.PromotionItems;

public class ProductCategorySetting
{
    public int ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; }
    public double Percentage { get; set; }
    public decimal MinTurnover { get; set; }
    public decimal MaxPayout { get; set; }
    public bool Unlimited { get; set; }
    public string ProductList { get; set; }
    public List<ProductDetail> Products { get; set; }
}

public class ProductDetail
{
    public int ProductId { get; set; }
    public int ProductCategoryId { get; set; }
    public int Order { get; set; }
    public string ProductName { get; set; }
}