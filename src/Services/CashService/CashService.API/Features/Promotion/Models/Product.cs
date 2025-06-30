namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int ParentId { get; set; }
    public Product[] ChildProducts { get; set; }
}