using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Services;

public interface IProductService
{
    IEnumerable<Product> GetProducts(GetProductsParam param);

    Task<IEnumerable<Product>> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param);

    //IEnumerable<ProductDetail> GetProductDetails(int custId, IEnumerable<Product> products);
}

public class ProductService(
    IDynamicRepository repository,
    ICacheService cacheService,
    Func<int, bool> canViewProduct,
    Func<int, string> getProductName) : IProductService
{
    private const int CustProductsCacheTimeInMinutes = 5;
    private const string ProductsCacheKey = "AlphaPlus.BetLists.Products";
    private readonly int[] IgnoreCheckProductStatusTypes = [41, 53];

    public IEnumerable<Product> GetProducts(GetProductsParam param)
    {
        var products = cacheService.GetOrSet(
            $"{ProductsCacheKey}-{param.SiteName}-{param.CurrencyId}-{param.Language}",
            () => repository.Fetch<Product>(new GetProductsCriteria(param.SiteName, param.CurrencyId, param.SettingStatus))
                .Where(product => !product.IsHide)
                .Select(product =>
                {
                    product.Name = getProductName(product.Id);
                    product.TypeName = product.Type == 0 ? string.Empty : getProductName(product.Type);
                    return product;
                }).ToList(),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

        return products.Where(i => IsRemainSupportProduct(i, param.MinReportDate) && (IgnoreCheckProductStatusTypes.Contains(i.Type) || canViewProduct(i.Id)));
    }

    public async Task<IEnumerable<Product>> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param)
    {
        var products = await repository.FetchAsync<Product>(new GetProductsByGroupHeadlineCriteria(param.CustId, param.SettingStatus));

        return products
            .Where(product => product.IsEnable && IsRemainSupportProduct(product, param.MinDate))
            .Select(product =>
                {
                    product.Name = getProductName(product.Id);
                    product.TypeName = product.Type == 0 ? string.Empty : getProductName(product.Type);
                    return product;
                });
    }

    private static bool IsRemainSupportProduct(Product product, DateTime minReportDate)
        => !product.EndOfSupportDate.HasValue
        || minReportDate <= product.EndOfSupportDate.Value;


}