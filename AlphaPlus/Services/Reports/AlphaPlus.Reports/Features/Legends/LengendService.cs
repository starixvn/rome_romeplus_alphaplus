using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Legends;

public interface ILengendService
{
    string GetProductNamesForLiveCasinoTooltip(GetProductsParam productsParam);

    string GetProductNamesForRNGCasinoTooltip(GetProductsParam productsParam);

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
    private static readonly HashSet<int> LiveCasinoTooltips =
    [
        ProductIdConstants.Allbet,
        ProductIdConstants.SexyGaming,
        ProductIdConstants.SAGaming,
        ProductIdConstants.GTCasino,
        ProductIdConstants.BBin,
        ProductIdConstants.ION,
        ProductIdConstants.WM,
        ProductIdConstants.GDG,
        ProductIdConstants.BigGame,
        ProductIdConstants.PPLiveCasino,
        ProductIdConstants.YeeBet,
        ProductIdConstants.FGG,
        ProductIdConstants.WE
    ];
    private static readonly HashSet<int> RNGCasinoTooltips =
    [
        ProductIdConstants.RNGWar,
        ProductIdConstants.SabaClub,
        ProductIdConstants.VGaming
    ];

    public string GetProductNamesForLiveCasinoTooltip(GetProductsParam productsParam)
        => GetProductNames(productsParam, LiveCasinoTooltips);

    public string GetProductNamesForRNGCasinoTooltip(GetProductsParam productsParam)
    {
        var productIds = productService.GetProducts(productsParam)
            .Where(product => product.Type == ProductIdConstants.DummyRNGProductId || RNGCasinoTooltips.Contains(product.Id))
            .Select(product => product.Id);

        return productService.ConcatProductNames(productIds);
    }

    public async Task<string> GetOfferedProductsNeedDelayTimeToSummarizeData(
        GetProductsParam productsParam,
        IEnumerable<int> offeredProductIds = null)
    {
        var productIdsNeedDelayTimeToSummarizeData = await GetProductsNeedDelayTimeToSummarizeData();

        if (offeredProductIds != null)
        {
            productIdsNeedDelayTimeToSummarizeData = productIdsNeedDelayTimeToSummarizeData
                .Where(productId => offeredProductIds.Contains(productId));
        }

        return GetProductNames(productsParam, new HashSet<int>(productIdsNeedDelayTimeToSummarizeData));
    }

    private string GetProductNames(GetProductsParam productsParam, HashSet<int> offeredProductIds)
    {
        var productIds = productService.GetProducts(productsParam)
            .Where(product => offeredProductIds.Contains(product.Id))
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
