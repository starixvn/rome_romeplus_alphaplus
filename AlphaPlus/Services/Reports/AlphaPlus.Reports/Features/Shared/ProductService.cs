using AlphaPlus.Reports.Features.Shared.Criteria;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Features.Shared;

public interface IProductService
{
    IEnumerable<Product> GetProgressiveJackpotProduct(string site, int currencyId, IEnumerable<int> progressiveJackpotproductIds);

    IEnumerable<Product> GetAllOfferedProducts(string site, int currencyId, DateTime? minReportDate = null);

    IEnumerable<Product> FilterProducts(string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null);

    IEnumerable<int> FilterProductIds(string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null);

    bool CanViewProduct(string site, int currencyId, int productId, DateTime? minReportDate = null);

    IEnumerable<string> GetProductNames(IEnumerable<int> productIds);

    string ConcatProductNames(string productIds);

    string ConcatProductNames(IEnumerable<int> productIds, IDictionary<int, string> replacedProductNames);
}

public class ProductService(
    IDynamicRepository repository,
    ICacheService cacheService,
    Func<int, bool> canViewProduct,
    ICacheRefService cacheRefService,
    IOptions<AppSettings> options) : IProductService
{
    private const int CustProductsCacheTimeInMinutes = 5;
    private const string ProductsCacheKey = "AlphaPlus.Reports.Products";

    public IEnumerable<Product> GetProgressiveJackpotProduct(string site, int currencyId, IEnumerable<int> progressiveJackpotproductIds)
        => FilterProducts(site, currencyId, progressiveJackpotproductIds);

    public IEnumerable<Product> GetAllOfferedProducts(
        string site,
        int currencyId,
        DateTime? minReportDate = null) => GetOfferedProducts(site, currencyId, minReportDate);

    public IEnumerable<Product> FilterProducts(string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null)
        => GetOfferedProducts(site, currencyId, minReportDate).Where(product => productIds.Contains(product.Id));

    public IEnumerable<int> FilterProductIds(string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null)
    {
        var products = productIds.Any()
            ? GetOfferedProducts(site, currencyId, minReportDate)
            : FilterProducts(site, currencyId, productIds, minReportDate);

        return products.Select(product => product.Id);
    }

    public bool CanViewProduct(string site, int currencyId, int productId, DateTime? minReportDate = null)
        => FilterProducts(site, currencyId, [productId], minReportDate).Any();

    public IEnumerable<string> GetProductNames(IEnumerable<int> productIds)
    {
        if (productIds == null)
        {
            yield break;
        }

        foreach (var id in productIds)
        {
            yield return cacheRefService.GetProductName(id);
        }
    }

    public string ConcatProductNames(string productIds)
        => string.Join(", ", GetProductNames(productIds.ToListInt()));

    public string ConcatProductNames(IEnumerable<int> productIds, IDictionary<int, string> replacedProductNames)
        => productIds == null
        ? string.Empty
        : string.Join(", ", productIds.Select(id => replacedProductNames.TryGetValue(id, out var name) ? name : cacheRefService.GetProductName(id)));

    private IEnumerable<Product> GetOfferedProducts(string site, int currencyId, DateTime? minReportDate)
    {
        var products = cacheService.GetOrSet(
            $"{ProductsCacheKey}-{site}-{currencyId}-{LanguageCode}",
            () => repository.Fetch<Product>(new GetProductsCriteria(site, currencyId, options.Value.SettingStatus))
                .Where(product => !product.IsHide)
                .Select(product =>
                {
                    product.DisplayName = cacheRefService.GetProductName(product.Id);
                    product.TypeName = product.Type == 0 ? string.Empty : cacheRefService.GetProductName(product.Type);
                    return product;
                }).ToList(),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

        return products.Where(i => canViewProduct(i.Id) && IsRemainSupportProduct(i, minReportDate));
    }

    private static bool IsRemainSupportProduct(Product product, DateTime? minReportDate)
        => !minReportDate.HasValue
        || !product.EndOfSupportDate.HasValue
        || minReportDate.Value <= product.EndOfSupportDate.Value;

    private static string LanguageCode => Thread.CurrentThread.CurrentCulture.Name;
}