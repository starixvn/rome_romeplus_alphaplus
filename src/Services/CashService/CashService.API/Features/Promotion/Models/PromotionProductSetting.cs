namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionProductSetting : BasePromotion
{   
    public int ProductCategoryId { get; set; }
    public double Percentage { get; set; }    
    public decimal MaxPayout { get; set; }    
    public string ProductList { get; set; }
    public List<Product> Products =>
        string.IsNullOrEmpty(ProductList)
            ? new List<Product>()
            : ProductList.Split(',')
                .Select(p => p.Trim())
                .Select(p => new Product
                {
                    ProductCategoryId = ProductCategoryId,
                    ProductId = Convert.ToInt32(p),
                    ProductName = "Product " + p
                })
                .ToList();
}