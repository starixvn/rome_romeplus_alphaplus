using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Features.Products;

public interface IProductService
{
    string ConcatProductNames(IEnumerable<int> productIds);

    Task<IEnumerable<Product>> GetProducts(GetProductsParam param);

    Task<IEnumerable<ProductDetail>> GetProductDetails(int custId, IEnumerable<Product> allOfferProducts);

    Task<string> GetProductsNeedDelayTimeToSummarizeData(
        GetProductsParam productParam,
        IEnumerable<int> enableJackpotProductIds = null);

    Task<string> GetProductNames(GetProductsParam productsParam, HashSet<int> offeredProductIds);

    string GetProductNamesExportExcel(
        IEnumerable<ProductDetail> productDetails,
        IEnumerable<int> selectedProductIds,
        IEnumerable<int> selectedResourceIds);
}

public class ProductService(
    IDynamicRepository repository,
    IMemoryCache cacheService,
    Func<int, bool> canViewProduct,
    ICacheRefService cacheRefService,
    IOptions<AppSettings> options) : IProductService
{
    private const int CustProductsCacheTimeInMinutes = 5;
    private const string ProductsCacheKey = "AlphaPlus.Reports.Products";
    private const int ProductDetailsCacheInMinutes = 60 * 24; // 1 day
    private const string ProductDetailsCacheKey = "AlphaPlus.Reports.ProductDetails";
    private const int CustProductDetailsCacheInMinutes = 5;
    private const int ProductsNeedDelayTimeToSummarizeDataCacheInMinutes = 5;
    private const string CacheKey = "AlphaPlus.Reports.Legends.ProductsNeedDelayTimeToSummarizeData";

    private readonly IDynamicRepository repository = repository;
    private readonly IMemoryCache cacheService = cacheService;
    private readonly Func<int, bool> canViewProduct = canViewProduct;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly AppSettings appSettings = options.Value;

    public string ConcatProductNames(IEnumerable<int> productIds)
        => productIds.Select(cacheRefService.GetProductName).JoinWithComma(", ");

    public async Task<IEnumerable<Product>> GetProducts(GetProductsParam param)
    {
        var products = await cacheService.GetOrCreateAsync(
            $"{ProductsCacheKey}-{param.SiteName}-{param.CurrencyId}-{param.Language}",
            async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes));
                var products = await repository.FetchAsync<Product>(new GetProductsCriteria(param.SiteName, param.CurrencyId, param.SettingStatus));

                return products
                    .Where(product => !product.IsHide)
                    .Select(product =>
                    {
                        product.Name = cacheRefService.GetProductName(product.Id);
                        product.TypeName = product.Type == 0 ? string.Empty : cacheRefService.GetProductName(product.Type);
                        return product;
                    }).ToList();
            });

        return products.Where(i => IsRemainSupportProduct(i, param.MinReportDate) && canViewProduct(i.Id));
    }

    private static bool IsRemainSupportProduct(Product product, DateTime? minReportDate)
        => !minReportDate.HasValue
        || !product.EndOfSupportDate.HasValue
        || minReportDate.Value <= product.EndOfSupportDate.Value;

    public Task<IEnumerable<ProductDetail>> GetProductDetails(int custId, IEnumerable<Product> allOfferProducts)
        => cacheService.GetOrCreateAsync(
            $"{ProductDetailsCacheKey}-{custId}",
            async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductDetailsCacheInMinutes));
                var productSupportDetail = allOfferProducts
                    .Where(product => IsSupportProductDetail(product.Id))
                    .Select(product => product.Id)
                    .ToList();

                return (await GetAllProductDetails())
                    .Where(p => productSupportDetail.Contains(p.ProductId) && CanViewProductDetailByResourceId(p.ResourceId))
                    .OrderBy(p => p.ProductId)
                    .ThenBy(p => p.Order)
                    .Select(product =>
                    {
                        product.DisplayName = cacheRefService.GetProductDetailName(product.ResourceId);
                        return product;
                    });
            });

    private bool CanViewProductDetailByResourceId(int resourceId)
        => appSettings.IsSiteLabel777
        ? !ProductDetailIdConstants.ExcludedProductDetailFor777.Contains(resourceId)
        : true;

    private async Task<IEnumerable<ProductDetail>> GetAllProductDetails()
        => await cacheService.GetOrCreateAsync(
            ProductDetailsCacheKey,
            async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(ProductDetailsCacheInMinutes));
                return (await repository.FetchAsync<ProductDetail>(new GetProductsDetailCriteria())).OrderBy(product => product.ProductId).ThenBy(product => product.Order);
            });

    public async Task<string> GetProductsNeedDelayTimeToSummarizeData(
        GetProductsParam productParam,
        IEnumerable<int> enableJackpotProductIds = null)
    {
        var productIdsNeedDelayTimeToSummarizeData = await cacheService.GetOrCreateAsync(
            CacheKey,
            async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(ProductsNeedDelayTimeToSummarizeDataCacheInMinutes));
                return (await repository.FetchAsync<ProductsByRefTable>(new GetProductsByRefTableCriteria(null))).Select(p => p.ProductId);
            });

        if (enableJackpotProductIds != null)
        {
            productIdsNeedDelayTimeToSummarizeData = productIdsNeedDelayTimeToSummarizeData
                .Where(productId => enableJackpotProductIds.Contains(productId));
        }

        return await GetProductNames(productParam, new HashSet<int>(productIdsNeedDelayTimeToSummarizeData));
    }

    public async Task<string> GetProductNames(GetProductsParam productsParam, HashSet<int> offeredProductIds)
    {
        var productIds = (await GetProducts(productsParam))
            .Where(product => offeredProductIds.Contains(product.Id))
            .Select(product => product.Id);

        return ConcatProductNames(productIds);
    }

    public string GetProductNamesExportExcel(
        IEnumerable<ProductDetail> productDetails,
        IEnumerable<int> selectedProductIds,
        IEnumerable<int> selectedResourceIds)
    {
        if (selectedProductIds == null)
        {
            return string.Empty;
        }

        var productNames = new Dictionary<int, string>();
        productDetails
            .GroupBy(p => p.ProductId)
            .ToList()
            .ForEach(group =>
            {
                if (IsSupportProductDetail(group.Key))
                {
                    var allResourceIds = group.Select(p => p.ResourceId).Distinct().ToList();
                    var intersectResourceIds = selectedResourceIds.Intersect(allResourceIds).ToList();

                    if (intersectResourceIds.Count > 0
                        && intersectResourceIds.Count != allResourceIds.Count)
                    {
                        productNames[group.Key] = intersectResourceIds.Select(id => cacheRefService.GetProductDetailName(id)).JoinWithComma(", ");
                    }
                }
            });

        return selectedProductIds
            .Select(id => productNames.TryGetValue(id, out var name) ? name : cacheRefService.GetProductName(id))
            .JoinWithComma(", ");
    }

    private bool IsSupportProductDetail(int productId)
        => appSettings.ProductOfferProductDetail.Contains(productId);
}