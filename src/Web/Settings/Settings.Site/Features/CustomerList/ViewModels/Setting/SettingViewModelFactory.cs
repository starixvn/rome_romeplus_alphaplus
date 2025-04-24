using AlphaPlus.Products.Configurations;
using AlphaPlus.Products.ViewModels;
using AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting
{
    public class SettingViewModelFactory : GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemViewModel>
    {
        private const int LastProductSettingItemIndex = 10000;
        private const int ResetPasswordDisplayAfterLastProduct = 1;
        private const int ResetSecurityDisplayAfterLastProduct = 2;
        private const int RemoveOTPDisplayAfterLastProduct = 3;

        public SettingViewModelFactory(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override IEnumerable<ISettingItemViewModel> GetNonProductViewModels(ISearchAccountInfoRequest request)
        {
            yield return new EditInfoViewModel(request);
            yield return new TransferViewModel(request);
            yield return new ResetPasswordViewModel(request, displayOrder: LastProductSettingItemIndex + ResetPasswordDisplayAfterLastProduct);
            yield return new ResetSecurityCodeViewModel(request, displayOrder: LastProductSettingItemIndex + ResetSecurityDisplayAfterLastProduct);
            yield return new RemoveOtpViewModel(request, displayOrder: LastProductSettingItemIndex + RemoveOTPDisplayAfterLastProduct);
        }

        protected override ISettingItemViewModel GetProductGroupViewModel(int productGroupId, ISearchAccountInfoRequest request)
            => new ProductGroupSettingItemViewModel(request, productGroupId);

        protected override ISettingItemViewModel GetProductViewModel(ProductItem productItem, ISearchAccountInfoRequest request)
        {
            throw new NotImplementedException();
        }
    }
}