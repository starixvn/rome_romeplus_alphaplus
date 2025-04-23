using System.Text;
using AlphaPlus.BetLists.Shared.Constants;
using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Helpers;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.BetLists.Shared.Services;

public interface IProductService
{
    IEnumerable<string> ConcatProductNames(IEnumerable<int> productIds, bool isSiteLabel777);

    Task<IEnumerable<Product>> GetProducts(GetProductsParam param);

    Task<IEnumerable<Product>> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param);

    Task<bool> IsEnabledJackpot(string site, int currencyId, bool isTest);

    Task<IEnumerable<ProductDetail>> GetProductDetails(GetProductDetailsParam param, IEnumerable<Product> allOfferProducts);

    string BuildProductDetailsXml(IEnumerable<ProductDetail> productDetails);

    Task<string> BuildProductDetailsXml789y(
        IEnumerable<Product> products,
        string userSelectedProductIds,
        string userSelectedResourceds,
        HashSet<int> productOfferProductDetail);
}

public class ProductService(
    IDynamicRepository repository,
    IMemoryCache cacheService,
    Func<int, bool> canViewProduct) : IProductService
{
    private const int CustProductsCacheTimeInMinutes = 5;
    private const string ProductsCacheKey = "AlphaPlus.BetLists.Products";
    private const int ProductDetailsCacheInMinutes = 60 * 24; // 1 day
    private const string ProductDetailsCacheKey = "AlphaPlus.BetLists.ProductDetails";
    private const int CustProductDetailsCacheInMinutes = 5;
    public const int CacheDayforJackpot = 7;

    public IEnumerable<string> ConcatProductNames(IEnumerable<int> productIds, bool isSiteLabel777)
        => productIds.Select(id => CacheRefServiceHelper.GetProductName(id, isSiteLabel777));

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
                        product.Name = CacheRefServiceHelper.GetProductName(product.Id, param.IsSiteLabel777);
                        product.TypeName = product.Type == 0 ? string.Empty : CacheRefServiceHelper.GetProductName(product.Type, param.IsSiteLabel777);
                        return product;
                    }).ToList();
            });

        return products.Where(i => IsRemainSupportProduct(i, param.MinReportDate) && canViewProduct(i.Id));
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

    public async Task<bool> IsEnabledJackpot(string site, int currencyId, bool isTest)
    {
        var (jackpotOfferedBySites, jackpotOfferedByCurrencies) = await GetEnabledJackpot(isTest);
        var productIdsBySite = jackpotOfferedBySites.Where(i => i.Site.Equals(site, StringComparison.OrdinalIgnoreCase));

        return jackpotOfferedByCurrencies.Any(i => i.Currency == currencyId && productIdsBySite.Any(s => s.Product == i.Product));
    }

    private Task<(IEnumerable<JackpotOfferedBySite>, IEnumerable<JackpotOfferedByCurrency>)> GetEnabledJackpot(bool isTest)
        => cacheService.GetOrCreateAsync(
        "AlphaPlus.BetLists.OfferedJackpot",
        cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromDays(CacheDayforJackpot));
            return repository.FetchMultipleAsync<JackpotOfferedBySite, JackpotOfferedByCurrency>(new GetEnableJackpotProductCriteria(isTest));
        });

    public Task<IEnumerable<ProductDetail>> GetProductDetails(GetProductDetailsParam param, IEnumerable<Product> allOfferProducts)
        => cacheService.GetOrCreateAsync(
        $"{ProductDetailsCacheKey}-{param.CustId}",
        async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductDetailsCacheInMinutes));
            var productSupportDetail = allOfferProducts
                .Where(product => param.ProductOfferProductDetail.Contains(product.Id))
                .Select(product => product.Id)
                .ToList();

            return (await GetAllProductDetails())
                .Where(p => productSupportDetail.Contains(p.ProductId) && CanViewProductDetailByResourceId(p.ResourceId, param.IsSiteLabel777))
                .OrderBy(p => p.ProductId)
                .ThenBy(p => p.Order)
                .Select(product =>
                {
                    product.DisplayName = CacheRefServiceHelper.GetProductDetailName(product.ResourceId, param.IsSiteLabel777);
                    return product;
                });
        });

    private static bool CanViewProductDetailByResourceId(int resourceId, bool isSiteLabel777)
        => isSiteLabel777
        ? !ProductDetailIdConstants.ExcludedProductDetailFor777.Contains(resourceId)
        : true;

    private Task<IEnumerable<ProductDetail>> GetAllProductDetails()
        => cacheService.GetOrCreateAsync(
        ProductDetailsCacheKey,
        cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(ProductDetailsCacheInMinutes));
            return repository.FetchAsync<ProductDetail>(new GetProductsDetailCriteria());
        });

    public string BuildProductDetailsXml(IEnumerable<ProductDetail> productDetails)
    {
        var xml = new StringBuilder("<ROOT>");
        foreach (var item in productDetails)
        {
            xml.Append(item.ToXml());
        }
        xml.Append("</ROOT>");

        return xml.ToString();
    }

    public async Task<string> BuildProductDetailsXml789y(
        IEnumerable<Product> products,
        string userSelectedProductIds,
        string userSelectedResourceds,
        HashSet<int> productOfferProductDetail)
    {
        var productSupportDetail = products
            .Where(product => productOfferProductDetail.Contains(product.Id))
            .Select(product => product.Id)
            .ToList();

        if ("*" != userSelectedProductIds)
        {
            var userSelectedProductIdsList = userSelectedProductIds.ToListInt().ToList();
            products = products.Where(p => userSelectedProductIdsList.Contains(p.Id));
        }

        var productIds = products.Select(p => p.Id).ToList();
        var userSelectedResourcedsList = userSelectedResourceds.ToListInt().ToList();
        var productDetails = new List<ProductDetail>();

        (await GetAllProductDetails()).ToList().ForEach(p =>
        {
            if (productSupportDetail.Contains(p.ProductId))
            {
                if (userSelectedResourcedsList.Contains(p.ResourceId))
                {
                    productDetails.Add(p);
                }
            }
            else if (productIds.Contains(p.ProductId))
            {
                productDetails.Add(p);
            }
        });

        return BuildProductDetailsXml(productDetails);
    }
}