using AlphaPlus.Features.Messages.API.Criteria.Product;
using Fanex.Caching;
using Fanex.Data.Repository;
using AlphaPlus.Features.Messages.API.Models.Product;

namespace AlphaPlus.Features.Messages.API.Services
{
    public interface ProductService
    {
        IEnumerable<Product> GetOfferedProductsByCustomer(int custId);

        IEnumerable<int> GetUnconfirmSettingProductIds(int custId);

        IEnumerable<byte> GetOfferedProductIdsByCustomer(int custId);
    }

    public class ProductServiceImpl : ProductService
    {
        private const int ExpiredTimeOfferredProductMinutes = 5;
        private readonly ICacheService cacheService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly Func<int, bool> canViewProduct;
        private const string ProductsCacheKey = "Alpha.Message.Services.GetAllProducts";
        private const string OfferedProductCacheKey = "Alpha.Message.Services.ProductServiceImpl.OfferedProduct";

        public ProductServiceImpl(
            ICacheService cacheService,
            IDynamicRepository dynamicRepository,
            Func<int, bool> canViewProduct)
        {
            this.cacheService = cacheService;
            this.dynamicRepository = dynamicRepository;
            this.canViewProduct = canViewProduct;
        }

        public IEnumerable<int> GetUnconfirmSettingProductIds(int custId)
            => dynamicRepository.Fetch<int>(new UnsynchronizedSettingCriteria(custId));

        public IEnumerable<byte> GetOfferedProductIdsByCustomer(int custId)
            => cacheService.GetOrSet(
                $"{OfferedProductCacheKey}.{custId}",
                () =>
                {
                    var products = dynamicRepository.Fetch<Product>(new GetOfferedProductCriteria(custId));

                    return products != null
                        ? products.Where(p => !p.IsHidden).Select(p => (byte)p.Id)
                        : Enumerable.Empty<byte>();
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(ExpiredTimeOfferredProductMinutes)));

        public IEnumerable<Product> GetOfferedProductsByCustomer(int custId)
           => GetAllProducts().Where(product => canViewProduct(product.Id));

        private IEnumerable<Product> GetAllProducts()
            => cacheService.GetOrSet(
                ProductsCacheKey,
                () => dynamicRepository.Fetch<Product>(new GetProductsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}