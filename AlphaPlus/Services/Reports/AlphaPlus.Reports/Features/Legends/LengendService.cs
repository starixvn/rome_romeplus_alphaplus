using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Legends;

public interface ILengendService
{
    Task<string> GetOfferedProductsNeedDelayTimeToSummarizeData(
        GetProductsParam productsParam,
        IEnumerable<int> offeredProductIds = null);
}

public class LengendService(
    IDynamicRepository repository,
    ICacheService cacheService,
    IProductService productService) : ILengendService
{
    private const int CacheInMinutes = 5;
    private const string CacheKey = "AlphaPlus.Reports.Legends.ProductsNeedDelayTimeToSummarizeData";

    public async Task<string> GetOfferedProductsNeedDelayTimeToSummarizeData(
        GetProductsParam productsParam,
        IEnumerable<int> offeredProductIds = null)
    {
        var productIdsNeedDelayTimeToSummarizeData = await GetProductsNeedDelayTimeToSummarizeData();

        if (offeredProductIds != null)
        {
            productIdsNeedDelayTimeToSummarizeData = productIdsNeedDelayTimeToSummarizeData
                .Where(productId => offeredProductIds.Contains(productId))
                .ToList();
        }

        var productIds = productService.GetProducts(productsParam)
            .Where(product => productIdsNeedDelayTimeToSummarizeData.Contains(product.Id))
            .Select(product => product.Id);

        return productService.ConcatProductNames(productIds);
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
