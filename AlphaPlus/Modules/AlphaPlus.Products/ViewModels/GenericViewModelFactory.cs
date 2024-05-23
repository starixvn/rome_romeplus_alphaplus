using AlphaPlus.Products.Configurations;
using AlphaPlus.Products.Models;
using Microsoft.Extensions.Configuration;

namespace AlphaPlus.Products.ViewModels
{
    public abstract class GenericViewModelFactory<TRequest, TView>(IConfiguration configuration)
        where TRequest : IRequest
        where TView : IViewModel
    {
        protected readonly IConfiguration configuration = configuration;
        protected ProductConfiguration productConfiguration = configuration.GetSection("ProductConfiguration").Get<ProductConfiguration>();

        public IEnumerable<TView> GetViewModels(TRequest request)
           => request?.IsValid == true
           ? GetNonProductViewModels(request)
               .Concat(GetProductGroupViewModels(request))
               .Concat(GetProductViewModels(request))
               .OrderBy(viewModel => viewModel.DisplayOrder)
           : [];

        internal IEnumerable<TView> GetProductViewModels(TRequest request)
            => GetEnabledProductItems(request)
            .Select(productItem => GetProductViewModel(productItem, request));

        protected virtual IEnumerable<TView> GetNonProductViewModels(TRequest request)
            => [];

        protected abstract TView GetProductViewModel(ProductItem productItem, TRequest request);

        protected IEnumerable<ProductItem> GetEnabledProductItems(TRequest request)
        {
            var enabledProductItems = GetEnabledProductItems(request.Predicate);

            return request
                .Actor
                .GetProductStatus()
                .Where(productStatus
                      => productStatus.CanBeSeen
                      && productStatus.ProductGroupId == null
                      && enabledProductItems.Any(productItem => productItem.ProductId == productStatus.ProductId))
                .Select(ps
                        => enabledProductItems.First(
                        productItem => productItem.ProductId == ps.ProductId));
        }

        protected IEnumerable<TView> GetProductGroupViewModels(TRequest request)
            => GetEnabledProductGroupIds(request)
            .Select(productGroupId => GetProductGroupViewModel(productGroupId, request));

        protected abstract TView GetProductGroupViewModel(int productGroupId, TRequest request);

        protected virtual IEnumerable<int> GetEnabledProductGroupIds(TRequest request)
        {
            var groupIds = request
                .Actor
                .GetProductStatus()
                .Where(productStatus => productStatus.CanBeSeen && productStatus.ProductGroupId != null)
                .Select(p => p.ProductGroupId.Value);

            return groupIds
                .Distinct();
        }

        public IEnumerable<ProductItem> GetEnabledProductItems(Func<ProductItem, bool> predicate, DateTime? currentDate = null)
            => productConfiguration.Products
               .Where(configItem =>
                   configItem.Enabled
                   && configItem.EndSupportDate >= (currentDate ?? DateTime.Now)
                   && (predicate == null || predicate(configItem)));
    }
}