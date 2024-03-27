using System.Text;
using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Reports.Features.Shared.Criteria;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Features.Shared;

public interface IProductService
{
    string ConcatProductNames(IEnumerable<int> productIds);

    IEnumerable<Product> GetProducts(GetProductsParam param);

    IEnumerable<ProductDetail> GetProductDetails(int custId, IEnumerable<Product> products);

    string BuildProductDetailXml(ProductDetail productDetail);
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
    private const int ProductDetailsCacheInMinutes = 60 * 24; // 1 day
    private const string ProductDetailsCacheKey = "AlphaPlus.Reports.ProductDetails";
    private const int CustProductDetailsCacheInMinutes = 5;

    public string ConcatProductNames(IEnumerable<int> productIds)
        => productIds.Select(cacheRefService.GetProductName).JoinWithComma(", ");

    public IEnumerable<Product> GetProducts(GetProductsParam param)
    {
        var products = cacheService.GetOrSet(
            $"{ProductsCacheKey}-{param.SiteName}-{param.CurrencyId}-{param.Language}",
            () => repository.Fetch<Product>(new GetProductsCriteria(param.SiteName, param.CurrencyId, param.SettingStatus))
                .Where(product => !product.IsHide)
                .Select(product =>
                {
                    product.Name = cacheRefService.GetProductName(product.Id);
                    product.TypeName = product.Type == 0 ? string.Empty : cacheRefService.GetProductName(product.Type);
                    return product;
                }).ToList(),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

        return products.Where(i => canViewProduct(i.Id) && IsRemainSupportProduct(i, param.MinReportDate));
    }

    private static bool IsRemainSupportProduct(Product product, DateTime? minReportDate)
        => !minReportDate.HasValue
        || !product.EndOfSupportDate.HasValue
        || minReportDate.Value <= product.EndOfSupportDate.Value;

    public IEnumerable<ProductDetail> GetProductDetails(int custId, IEnumerable<Product> products)
        => cacheService.GetOrSet(
            $"{ProductDetailsCacheKey}-{custId}",
            () =>
            {
                var productSupportDetail = products
                    .Where(product => options.Value.ProductOfferProductDetail.Contains(product.Id))
                    .Select(product => product.Id)
                    .ToList();

                return GetAllProductDetails()
                    .Where(p => productSupportDetail.Contains(p.ProductId) && SportsbookSupportDetail(p))
                    .Select(product =>
                    {
                        product.DisplayName = GetProductDetailName(product.RefId, product.ResourceId);
                        return product;
                    });
            },
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductDetailsCacheInMinutes)));

    private string GetProductDetailName(string refId, int resourceId)
    {
        string productDetailName = null;

        if (options.Value.IsSiteLabel777 && ProductDetaiIdConstants.SportsbookDetailFor777Site.Contains(resourceId))
        {
            productDetailName = cacheRefService.GetResourceDataByRefIdAndResourceId($"{refId}_Saba", resourceId);
        }

        return string.IsNullOrWhiteSpace(productDetailName)
            ? cacheRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId)
            : productDetailName;
    }

    private bool SportsbookSupportDetail(ProductDetail detail)
    {
        if (options.Value.IsSiteLabel777 && detail.ProductId == ProductIdConstants.Sportsbook)
        {
            return ProductDetaiIdConstants.SportsbookDetailFor777Site.Contains(detail.ResourceId);
        }

        return true;
    }

    private IEnumerable<ProductDetail> GetAllProductDetails()
        => cacheService.GetOrSet(
            ProductDetailsCacheKey,
            () => repository.Fetch<ProductDetail>(new GetProductsDetailCriteria()).OrderBy(product => product.ProductId).ThenBy(product => product.Order),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(ProductDetailsCacheInMinutes)));

    public string BuildProductDetailXml(ProductDetail productDetail)
    {
        var xml = new StringBuilder();
        foreach (var leagueId in productDetail.LeagueGroupIds)
        {
            foreach (var typeId in productDetail.TicketTypeIds)
            {
                xml.AppendFormat(
                    "<ProductDetail ProductId=\"{0}\" Sporttype=\"{1}\" Bettype=\"{2}\" LeagueGroupId=\"{3}\" TicketType=\"{4}\"/>",
                    productDetail.ProductId,
                    productDetail.SportType,
                    productDetail.BetType,
                    leagueId,
                    typeId);
            }
        }

        return xml.ToString();
    }
}