using AlphaPlus.Features.Messages.API.Criteria.Product;
using AlphaPlus.Features.Messages.API.Models.Product;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Features.Messages.API.Services
{
    public interface ProductService
    {
        IEnumerable<Product> GetOfferedProductsByCustomer(int custId);

        IEnumerable<int> GetUnConfirmSettingProductIds(int custId);

        IEnumerable<byte> GetOfferedProductIdsByCustomer(int custId);
    }

    public class ProductServiceImpl : ProductService
    {
        private const int ExpiredTimeOfferedProductMinutes = 5;
        private readonly IMemoryCache cacheService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly Func<int, bool> canViewProduct;
        private const string ProductsCacheKey = "Alpha.Message.Services.GetAllProducts";
        private const string OfferedProductCacheKey = "Alpha.Message.Services.ProductServiceImpl.OfferedProduct";

        public ProductServiceImpl(
            IMemoryCache cacheService,
            IDynamicRepository dynamicRepository,
            Func<int, bool> canViewProduct)
        {
            this.cacheService = cacheService;
            this.dynamicRepository = dynamicRepository;
            this.canViewProduct = canViewProduct;
        }

        public IEnumerable<int> GetUnConfirmSettingProductIds(int custId)
            => dynamicRepository.Fetch<int>(new UnSynchronizedSettingCriteria(custId));

        public IEnumerable<byte> GetOfferedProductIdsByCustomer(int custId)
            => cacheService.GetOrCreate(
                $"{OfferedProductCacheKey}.{custId}",
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(ExpiredTimeOfferedProductMinutes));
                    var products = dynamicRepository.Fetch<Product>(new GetOfferedProductCriteria(custId));

                    return products != null
                        ? products.Where(p => !p.IsHidden).Select(p => (byte)p.Id)
                        : [];
                });

        public IEnumerable<Product> GetOfferedProductsByCustomer(int custId)
           => GetAllProducts().Where(product => canViewProduct(product.Id));

        private IEnumerable<Product> GetAllProducts()
            => cacheService.GetOrCreate(
                ProductsCacheKey,
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return dynamicRepository.Fetch<Product>(new GetProductsCriteria());
                });
    }
}