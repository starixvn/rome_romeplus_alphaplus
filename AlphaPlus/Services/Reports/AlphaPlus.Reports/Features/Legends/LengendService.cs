using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Legends;

public interface ILengendService
{
    Task<string> GetOfferedProductsNeedDelayTimeToSummarizeData(
        string site,
        int currencyId,
        DateTime? minReportDate = null,
        IEnumerable<int> filteredProductIds = null);
}

public class LengendService(
    IDynamicRepository repository,
    ICacheService cacheService,
    IProductService productService) : ILengendService
{
    private const int CacheInMinutes = 5;
    private const string CacheKey = "AlphaPlus.Reports.Legends.ProductsNeedDelayTimeToSummarizeData";

    public async Task<string> GetOfferedProductsNeedDelayTimeToSummarizeData(
        string site,
        int currencyId,
        DateTime? minReportDate = null,
        IEnumerable<int> filteredProductIds = null)
    {
        var productIdsNeedDelayTimeToSummarizeData = await GetProductsNeedDelayTimeToSummarizeData();

        if (filteredProductIds != null)
        {
            productIdsNeedDelayTimeToSummarizeData = productIdsNeedDelayTimeToSummarizeData.Where(productId => filteredProductIds.Contains(productId));
        }

        var productIds = productService.FilterProductIds(site, currencyId, productIdsNeedDelayTimeToSummarizeData.ToList(), minReportDate);

        return productService.GetProductNames(productIds).JoinWithComma(", ");
    }

    private Task<IEnumerable<int>> GetProductsNeedDelayTimeToSummarizeData()
        => cacheService.GetOrSetAsync(
        CacheKey,
        async () =>
        {
            var products = await repository.FetchAsync<ProductsByRefTable>(new GetProductsByRefTableCriteria(null));
            return products.Select(p => p.ProductId);
        },
        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheInMinutes)));
}
