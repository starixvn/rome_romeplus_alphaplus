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
               .OrderBy(viewModel => viewModel.DisplayOrder)
           : [];


        protected virtual IEnumerable<TView> GetNonProductViewModels(TRequest request)
            => [];

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
    }
}