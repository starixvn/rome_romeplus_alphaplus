namespace AlphaPlus.CashService.Site.Features.Promotion.Models.Products;
public class ProductDropdown
{
    public int ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; }
    public List<Product> Products { get; set; }
}