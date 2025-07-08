namespace AlphaPlus.CashService.Site.Features.Promotion.Models.Products;

using AlphaPlus.Shared.AppResources.CashServices;
using Enum;
using Helpers;

public class ProductDropdown
{
    public int ProductCategoryId { get; set; }
    public string ProductCategoryName => EnumHelper.GetEnumDescriptionByNumber<ProductCategory>(ProductCategoryId, CashServicesResx.ResourceManager);
    public List<Product> Products { get; set; }
}