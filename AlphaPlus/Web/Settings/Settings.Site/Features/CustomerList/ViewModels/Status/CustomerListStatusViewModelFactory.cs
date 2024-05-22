using AlphaPlus.Products.Configurations;
using AlphaPlus.Products.ViewModels;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status
{
    public class CustomerListStatusViewModelFactory
        : GenericViewModelFactory<ISearchAccountInfoRequest, IStatusItemViewModel>
    {
        public CustomerListStatusViewModelFactory(IOptions<ProductConfiguration> options)
            : base(options)
        {
        }

        protected override IEnumerable<IStatusItemViewModel> GetNonProductViewModels(ISearchAccountInfoRequest request)
        {
            yield return new ConfirmSuspendedViewModel(request);
            yield return new ConfirmClosedViewModel(request);
            yield return new ConfirmAllowOutrightViewModel(request);
            yield return new ConfirmAllowAutoMasterPTViewModel(request);
            yield return new ConfirmDoubleCommViewModel(request);
        }

        protected override IStatusItemViewModel GetProductGroupViewModel(int productGroupId, ISearchAccountInfoRequest request)
        {
            throw new NotImplementedException();
        }

        protected override IStatusItemViewModel GetProductViewModel(ProductItem productItem, ISearchAccountInfoRequest request)
            => new ProductStatusItemViewModel(request, productItem);
    }
}