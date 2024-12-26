using AlphaPlus.Products.Configurations;
using AlphaPlus.Products.ViewModels;
using AlphaPlus.Settings.MemberInfo.API.Services;
using Fanex.Data.Repository;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Multiple;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting
{
    public class SettingMultipleViewModelFactory : GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemMultipleViewModel>
    {
        private readonly IAccountService accountService;

        public SettingMultipleViewModelFactory(IConfiguration configuration)
            : base(configuration)
        {
            accountService = new AccountService(new DynamicRepository());
        }

        protected override IEnumerable<ISettingItemMultipleViewModel> GetNonProductViewModels(ISearchAccountInfoRequest request)
        {
            yield return new DoubleCommMultipleViewModel(request);
            yield return new SuspendedMultipleViewModel(request);
            yield return new ClosedMultipleViewModel(request);
            yield return new ProtectedMultipleViewModel(request);

            if (request.SupportAutoTransfer &&
                (!request.IsSubAcc || request.SubPageAccess.Transfer))
            {
                yield return new AutoTransferMultipleViewModel(request, accountService);
            }
        }

        protected override ISettingItemMultipleViewModel GetProductGroupViewModel(int productGroupId, ISearchAccountInfoRequest request)
        {
            return new ProductGroupMultipleSettingItemViewModel(request, productGroupId);
        }
    }
}