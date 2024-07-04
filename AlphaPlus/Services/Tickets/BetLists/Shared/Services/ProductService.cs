using System.Text;
using AlphaPlus.BetLists.Shared.Constants;
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

    Task<bool> IsEnabledJackpot(string site, int currencyId, bool isTest);

    Task<IEnumerable<ProductDetail>> GetProductDetails(GetProductDetailsParam param, IEnumerable<Product> products);

    string BuildProductDetailsXml(IEnumerable<ProductDetail> productDetails);

    string BuildProductDetailXml(ProductDetail productDetail);
}

public class ProductService(
    IDynamicRepository repository,
    ICacheService cacheService,
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

        return products.Where(i => IsRemainSupportProduct(i, param.MinReportDate) && (ProductIdConstants.IgnoreCheckProductStatusTypes.Contains(i.Type) || canViewProduct(i.Id)));
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
        => cacheService.GetOrSetAsync(
        "AlphaPlus.BetLists.OfferedJackpot",
        () => repository.FetchMultipleAsync<JackpotOfferedBySite, JackpotOfferedByCurrency>(new GetEnableJackpotProductCriteria(isTest)),
        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheDayforJackpot)));

    public Task<IEnumerable<ProductDetail>> GetProductDetails(GetProductDetailsParam param, IEnumerable<Product> products)
        => cacheService.GetOrSetAsync(
        $"{ProductDetailsCacheKey}-{param.CustId}",
        async () =>
        {
            var productSupportDetail = products
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
        },
        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductDetailsCacheInMinutes)));

    private static bool CanViewProductDetailByResourceId(int resourceId, bool isSiteLabel777)
        => isSiteLabel777
        ? !ProductDetailIdConstants.ExcludedProductDetailFor777.Contains(resourceId)
        : true;

    private Task<IEnumerable<ProductDetail>> GetAllProductDetails()
        => cacheService.GetOrSetAsync(
        ProductDetailsCacheKey,
        () => repository.FetchAsync<ProductDetail>(new GetProductsDetailCriteria()),
        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(ProductDetailsCacheInMinutes)));

    public string BuildProductDetailsXml(IEnumerable<ProductDetail> productDetails)
    {
        var xml = new StringBuilder("<ROOT>");
        foreach (var item in productDetails)
        {
            xml.Append(BuildProductDetailXml(item));
        }
        xml.Append("</ROOT>");

        return xml.ToString();
    }

    public string BuildProductDetailXml(ProductDetail productDetail)
        => string.Format(
        "<ProductDetail ProductId=\"{0}\" Sporttype=\"{1}\" Bettype=\"{2}\" LeagueGroupId=\"{3}\" TicketType=\"{4}\"/>",
        productDetail.ProductId,
        productDetail.SportType,
        productDetail.BetType,
        productDetail.LeagueGroupId,
        productDetail.TicketType);
}