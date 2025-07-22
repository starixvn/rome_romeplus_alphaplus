using AlphaPlus.Core.Configurations;
using AlphaPlus.Products.Configurations;
using AlphaPlus.Products.ViewModels;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status
{
    public sealed class StatusViewModelFactory
        : GenericViewModelFactory<ISearchAccountInfoRequest, IStatusItemViewModel>
    {
        private readonly IAccountService accountService;

        public StatusViewModelFactory(IConfiguration configuration)
            : base(configuration)
        {
            accountService = new AccountService(new DynamicRepository());
        }

        protected override IEnumerable<IStatusItemViewModel> GetNonProductViewModels(ISearchAccountInfoRequest request)
        {
            yield return new ConfirmSuspendedViewModel(request);
            yield return new ConfirmClosedViewModel(request);
            yield return new ConfirmUnlockAccessViewModel(request);
            yield return new ConfirmProtectedViewModel(request);
            yield return new ConfirmAllowOutrightViewModel(request);
            yield return new ConfirmAllowAutoMasterPTViewModel(request);
            yield return new ConfirmDoubleCommViewModel(request);
            yield return new ConfirmTransferBalanceDailyViewModel(request);

            if (!request.SearchAccount.IsCashMember && request.SupportAutoTransfer &&
                (!request.IsSubAcc || request.SubPageAccess.Transfer))
            {
                yield return new ConfirmAllowAutoTransferViewModel(request, accountService);
            }
        }

        protected override IStatusItemViewModel GetProductGroupViewModel(int productGroupId, ISearchAccountInfoRequest request)
            => new ProductGroupStatusItemViewModel(request, productGroupId);

        protected override IStatusItemViewModel GetProductViewModel(ProductItem productItem, ISearchAccountInfoRequest request)
          => new ProductStatusItemViewModel(request, productItem);

        protected override IEnumerable<int> GetEnabledProductGroupIds(ISearchAccountInfoRequest request)
        {
            if (AppSettings.IsSiteLabel789y)
            {
                return [];
            }

            return base.GetEnabledProductGroupIds(request);
        }
    }
}