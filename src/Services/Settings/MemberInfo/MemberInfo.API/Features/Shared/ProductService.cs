using AlphaPlus.Settings.MemberInfo.API.Features.Shared.Criteria;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.MemberInfo.API.Features.Shared;

public interface IProductService
{
    Task<IEnumerable<Product>> GetPTProducts(int custId, byte settingStatus, DateTime? minDate = null);

    Task<IEnumerable<Product>> GetCommProducts(int custId, byte settingStatus, DateTime? minDate = null);
}

public class ProductService(
    CacheRefService cacheRefService,
    IMemoryCache cacheService,
    IDynamicRepository repository) : IProductService
{
    private const int CustProductsCacheTimeInMinutes = 5;
    private const string ProductsCacheKey = "AlphaPlus.MemberInfo.PTCommProducts.{0}.{1}";

    private readonly IMemoryCache cacheService = cacheService;
    private readonly IDynamicRepository repository = repository;

    public Task<IEnumerable<Product>> GetPTProducts(int custId, byte settingStatus, DateTime? minDate = null)
        => GetPTCommProducts(custId, settingStatus, 1, minDate);

    public Task<IEnumerable<Product>> GetCommProducts(int custId, byte settingStatus, DateTime? minDate = null)
        => GetPTCommProducts(custId, settingStatus, 2, minDate);

    private async Task<IEnumerable<Product>> GetPTCommProducts(int custId, byte settingStatus, byte type, DateTime? minDate)
    {
        var products = await cacheService.GetOrCreateAsync(
            string.Format(ProductsCacheKey, type, custId),
            async (cacheEntry) =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes));
                var rawdata = await repository.FetchAsync<Product>(new GetPTCommProductsCriteria(custId, settingStatus, type));
                return rawdata.Where(product => IsRemainSupportProduct(product, minDate));
            });

        return products.Select(product =>
        {
            product.Name = cacheRefService.GetProductName(product.Id);
            return product;
        });
    }

    private static bool IsRemainSupportProduct(Product product, DateTime? minDate)
        => !product.EndOfSupportDate.HasValue || minDate <= product.EndOfSupportDate;
}
