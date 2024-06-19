using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Helpers;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Services;

public interface IProductService
{
    IEnumerable<string> ConcatProductNames(IEnumerable<int> productIds, bool isSiteLabel777);

    IEnumerable<Product> GetProducts(GetProductsParam param);

    Task<IEnumerable<Product>> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param);

    //IEnumerable<ProductDetail> GetProductDetails(int custId, IEnumerable<Product> products);
}

public class ProductService(
    IDynamicRepository repository,
    ICacheService cacheService,
    Func<int, bool> canViewProduct) : IProductService
{
    private const int CustProductsCacheTimeInMinutes = 5;
    private const string ProductsCacheKey = "AlphaPlus.BetLists.Products";
    private const int LiveCasinoProductId = 41;
    private const int RNGCasinoProductId = 53;
    private static readonly HashSet<int> IgnoreCheckProductStatusTypes = [LiveCasinoProductId, RNGCasinoProductId];

    public IEnumerable<string> ConcatProductNames(IEnumerable<int> productIds, bool isSiteLabel777)
        => productIds.Select(id => CacheRefServiceHelper.GetProductName(id, isSiteLabel777));

    public IEnumerable<Product> GetProducts(GetProductsParam param)
    {
        var products = cacheService.GetOrSet(
            $"{ProductsCacheKey}-{param.SiteName}-{param.CurrencyId}-{param.Language}",
            () => repository.Fetch<Product>(new GetProductsCriteria(param.SiteName, param.CurrencyId, param.SettingStatus))
                .Where(product => !product.IsHide)
                .Select(product =>
                {
                    product.Name = CacheRefServiceHelper.GetProductName(product.Id, param.IsSiteLabel777);
                    product.TypeName = product.Type == 0 ? string.Empty : CacheRefServiceHelper.GetProductName(product.Type, param.IsSiteLabel777);
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
                    product.Name = CacheRefServiceHelper.GetProductName(product.Id, param.IsSiteLabel777);
                    product.TypeName = product.Type == 0 ? string.Empty : CacheRefServiceHelper.GetProductName(product.Type, param.IsSiteLabel777);
                    return product;
                });
    }

    private static bool IsRemainSupportProduct(Product product, DateTime minReportDate)
        => !product.EndOfSupportDate.HasValue
        || minReportDate <= product.EndOfSupportDate.Value;


}