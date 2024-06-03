using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels;
using AlphaPlus.Utilities.IP;
using Fanex.Common;
using MediatR;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.MemberInfo.API.Services.Transfer;
using AlphaPlus.SecurityCode.Services;
using Newtonsoft.Json;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.CustomerList.Models;
using AlphaPlus.Products.Models;
using AlphaPlus.Settings.Site.Features.CustomerList.Params;
using AlphaPlus.Products.ViewModels;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;
using AlphaPlus.Settings.Site.Features.Shared;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using AlphaPlus.Settings.Site.Features.Shared.Models;
using AlphaPlus.Settings.Site.Features.Shared.Services;

namespace AlphaPlus.Settings.Site.Features.CustomerList
{
    public class CustomerListHandler :
        IRequestHandler<CustomerListRequest, CustomerListViewModel>,
        IRequestHandler<FetchDownlinesListRequest, CustomerListPagingViewModel>,
        IRequestHandler<CustomerStatusQuery, CustomerStatusViewModel>,
        IRequestHandler<CustomerStatusMultipleQuery, IEnumerable<ISettingItemMultipleViewModel>>,
        IRequestHandler<CustomerNonProductStatusCommand, Result>,
        IRequestHandler<MultipleDoubleCommRequest, MultipleDoubleCommViewModel>,
        IRequestHandler<UpdateMultipleDoubleCommCommand, IEnumerable<Result>>
    {
        private readonly CustomerListService customerListService;
        private readonly GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemMultipleViewModel> settingMultipleViewModelFactory;
        private readonly GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemViewModel> settingViewModelFactory;
        private readonly GenericViewModelFactory<ISearchAccountInfoRequest, IStatusItemViewModel> statusViewModelFactory;
        private readonly ISecurityCodeChecker securityCodeChecker;
        private readonly IBrowserHelper browserHelper;
        private readonly IPService ipService;
        private readonly TransferService transferService;
        private readonly ICacheRefService cacheRefService;

        private readonly string CurrenciesOfferAutoTransfer = AppSettings.GetConfigValue("OfferAutoTransfer.Currencies");

        public CustomerListHandler(
            CustomerListService customerListService,
            GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemMultipleViewModel> settingMultipleViewModelFactory,
            GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemViewModel> settingViewModelFactory,
            GenericViewModelFactory<ISearchAccountInfoRequest, IStatusItemViewModel> statusViewModelFactory,
            IPService ipService,
            ISecurityCodeChecker securityCodeChecker,
            IBrowserHelper browserHelper,
            TransferService transferService,
            ICacheRefService cacheRefService)
        {
            this.customerListService = customerListService;
            this.settingMultipleViewModelFactory = settingMultipleViewModelFactory;
            this.settingViewModelFactory = settingViewModelFactory;
            this.statusViewModelFactory = statusViewModelFactory;

            this.ipService = ipService;
            this.securityCodeChecker = securityCodeChecker;
            this.browserHelper = browserHelper;
            this.transferService = transferService;
            this.cacheRefService = cacheRefService;
        }

        public async Task<CustomerListViewModel> Handle(CustomerListRequest message, CancellationToken cancellation)
        {
            message.BreadcrumbLevel = !string.IsNullOrWhiteSpace(AlphaContext.Current.Request.Query["BreadcrumbLevelJson"]) ?
                JsonConvert.DeserializeObject<IEnumerable<CustInfo>>(AlphaContext.Current.Request.Query["BreadcrumbLevelJson"]) :
                    [];

            var breadcrumbLevel = new List<CustInfo>();

            if (message.BreadcrumbLevel != null && message.BreadcrumbLevel.Any())
            {
                foreach (var custInfo in message.BreadcrumbLevel)
                {
                    if (custInfo.CustLevelId == message.CustLevelId)
                    {
                        break;
                    }

                    breadcrumbLevel.Add(custInfo);
                }
            }

            breadcrumbLevel.Add(new CustInfo(message.CustId, message.CustName, message.CustLevelId));

            var customerListPagingViewModel = await Handle(
                new FetchDownlinesListRequest
                {
                    CustName = message.CustName,
                    CustId = message.CustId,
                    CustLevelId = message.CustLevelId,
                    Username = message.Username,
                    Status = message.Status,
                    DoubleComm = message.DoubleComm,
                    UnconfirmSetting = message.UnconfirmSetting,
                    PageIndex = message.PageIndex,
                    PageSize = message.PageSize,
                    AllowAutoTransfer = message.AllowAutoTransfer,
                },
                cancellation
             );

            message.EnableAllDayBalanceTransfer = transferService.IsAllDayBalanceTransfer(message.CustId);

            return new CustomerListViewModel(
                message,
                breadcrumbLevel,
                browserHelper.IsMobile(isOnlyTablet: false),
                message.LimitedDateToShowInfo,
                IsEnableAutoTransfer(message, CurrenciesOfferAutoTransfer),
                customerListPagingViewModel: customerListPagingViewModel);
        }

        public async Task<CustomerListPagingViewModel> Handle(FetchDownlinesListRequest message, CancellationToken cancellation)
        {
            var param = new CustomerListParam(
                    message.ViewerId,
                    message.CustLevel,
                    message.CustId,
                    message.Username,
                    message.Enabled,
                    message.Closed,
                    message.Suspended,
                    message.Locked,
                    message.DoubleComm,
                    message.PageSize,
                    message.PageIndex,
                    message.LimitedDateToShowInfo,
                    message.ShowNickname,
                    message.IsShowMyanmarOdds,
                    message.AllowAutoTransfer);

            return new CustomerListPagingViewModel(await customerListService.GetCustomerList(param), message, ipService);
        }

        public Task<CustomerStatusViewModel> Handle(CustomerStatusQuery message, CancellationToken cancellation)
            => Task.FromResult(BuildCustomerStatus(message));

        public Task<IEnumerable<ISettingItemMultipleViewModel>> Handle(CustomerStatusMultipleQuery message, CancellationToken cancellation)
           => Task.FromResult(BuildCustomerStatusMultiple(message));

        public Task<Result> Handle(CustomerNonProductStatusCommand message, CancellationToken cancellation)
          => customerListService.UpdateCustomerStatus(
              new UpdateCustomerStatusParam(
                  message.SearchAccountId,
                  message.ViewerId,
                  message.SubAccountId,
                  message.Language,
                  message.IP,
                  message.StatusValue,
                  message.StatusName,
                  message.ViewerUsername));

        public Task<MultipleDoubleCommViewModel> Handle(MultipleDoubleCommRequest message, CancellationToken cancellation)
           => Task.FromResult(new MultipleDoubleCommViewModel(message));

        public async Task<IEnumerable<Result>> Handle(UpdateMultipleDoubleCommCommand message, CancellationToken cancellation)
        {
            var results = new List<Result>();

            foreach (var searchAccountId in message.SearchAccountIds.Split(','))
            {
                results.Add(await customerListService.UpdateCustomerStatus(
                    new UpdateCustomerStatusParam(
                      Converter.ToInt(searchAccountId),
                      message.ViewerId,
                      message.SubAccountId,
                      message.Language,
                      message.IP,
                      message.EnabledDoubleComm,
                      StatusNames.DoubleComm,
                      message.ViewerUsername)));
            }

            return results;
        }

        internal CustomerStatusViewModel BuildCustomerStatus(CustomerStatusQuery message)
        {
            var customerStatusModel = customerListService.GetCustomerStatus(message.SearchAccountId, message.SearchAccountRoleId);
            var isAllDayBalTransfer = transferService.IsAllDayBalanceTransfer(message.ViewerId);

            var actor = new Products.Models.Customer(
               message.ViewerId,
               message.SubAccountId,
               message.ViewerUsername,
               message.ViewerLevelId,
               BuildActorProductStatus(customerStatusModel.DCSProducts.ToList()));

            var upline = new Products.Models.Customer(
                 message.UplineId,
                 message.SubAccountId,
                 message.UplineName,
                 message.UplineRoleId,
                 BuildUplineProductStatus(customerStatusModel.DCSProducts.ToList()));

            var searchAccount = new Products.Models.Customer(
               message.SearchAccountId,
               message.SubAccountId,
               message.SearchAccountName,
               message.SearchAccountRoleId,
               BuildSearchAccountProductStatus(customerStatusModel.DCSProducts.ToList()),
               upline);

            return new CustomerStatusViewModel(
                BuildCustomerProductStatus(actor, searchAccount, message, customerStatusModel, isAllDayBalTransfer),
                BuildCustomerProductSettings(actor, searchAccount, message, customerStatusModel));
        }

        internal IEnumerable<ISettingItemMultipleViewModel> BuildCustomerStatusMultiple(CustomerStatusMultipleQuery message)
        {
            var customerStatusModel = customerListService.GetCustomerStatus(message.SearchAccountId, message.SearchAccountRoleId);

            var actor = new Products.Models.Customer(
               message.ViewerId,
               message.SubAccountId,
               message.ViewerUsername,
               message.ViewerLevelId,
               BuildActorProductStatus(customerStatusModel.DCSProducts.ToList()));

            var upline = new Products.Models.Customer(
                 message.UplineId,
                 message.SubAccountId,
                 message.UplineName,
                 message.UplineRoleId,
                 BuildUplineProductStatus(customerStatusModel.DCSProducts.ToList()));

            var searchAccount = new Products.Models.Customer(
               message.SearchAccountId,
               message.SubAccountId,
               message.SearchAccountName,
               message.SearchAccountRoleId,
               BuildSearchAccountProductStatus(customerStatusModel.DCSProducts.ToList()),
               upline);

            return BuildCustomerProductMultipleSettings(actor, searchAccount, message, customerStatusModel);
        }

        internal static IEnumerable<CustomerProductStatus> BuildSearchAccountProductStatus(IEnumerable<DCSProductStatusModel> dcsProducts)
            => dcsProducts.Select(product => new CustomerProductStatus(product.ProductId, product.IsDisabled, product.IsHidden, product.GroupId));

        internal static IEnumerable<CustomerProductStatus> BuildUplineProductStatus(IEnumerable<DCSProductStatusModel> dcsProducts)
            => dcsProducts.Select(product => new CustomerProductStatus(product.ProductId, product.IsDisabledUpline, product.IsHiddenUpline, product.GroupId));

        internal static IEnumerable<CustomerProductStatus> BuildActorProductStatus(IEnumerable<DCSProductStatusModel> dcsProducts)
            => dcsProducts.Select(product => new CustomerProductStatus(product.ProductId, product.IsDisabled, product.IsHidden, product.GroupId));

        internal IEnumerable<IStatusItemViewModel> BuildCustomerProductStatus(
            Products.Models.Customer actor,
            Products.Models.Customer searchAccount,
            CustomerStatusQueryBase message,
            CustomerStatusModel model,
            bool allowedAllDayBalTransfer)
        {
            var request = new StatusSearchAccountRequest(
                actor,
                searchAccount,
                message.Language,
                message.IsDarkMode,
                cacheRefService.GetProductGroupName,
                cacheRefService.GetProductName)
            {
                SubPageAccess = message.SubPageAccess
            };

            request
                .AddData(DataKey.Actor.HasUpdateAccountPermission, message.CanUpdateAccount)
                .AddData(DataKey.SearchAccount.IsAllowedMasterAutoPT, !model.NonProduct.DisableAutoMPT)
                .AddData(DataKey.SearchAccount.Upline.IsAllowedOutright, !model.NonProduct.UplineDisableOutright)
                .AddData(DataKey.SearchAccount.IsAllowedOutright, !model.NonProduct.DisableOutright)
                .AddData(DataKey.SearchAccount.Upline.IsClosed, model.NonProduct.UplineClosed)
                .AddData(DataKey.SearchAccount.IsClosed, model.NonProduct.Closed)
                .AddData(DataKey.SearchAccount.IsDirectClosed, model.NonProduct.IsDirectClosed)
                .AddData(DataKey.SearchAccount.IsLockedAccess, model.NonProduct.Locked)
                .AddData(DataKey.SearchAccount.IsOfferProtectedMode, message.IsOfferProtectedMode)
                .AddData(DataKey.SearchAccount.IsProtectedMode, model.NonProduct.IsProtectedMode)
                .AddData(DataKey.SearchAccount.Upline.IsAllowedDoubleComm, model.NonProduct.UplineDoubleCommLosing)
                .AddData(DataKey.SearchAccount.IsAllowedDoubleComm, model.NonProduct.DoubleCommLosing)
                .AddData(DataKey.SearchAccount.Upline.IsSuspended, model.NonProduct.UplineSuspend)
                .AddData(DataKey.SearchAccount.IsSuspended, model.NonProduct.Suspended)
                .AddData(DataKey.SearchAccount.IsDirectSuspend, model.NonProduct.IsDirectSuspend)
                .AddData(DataKey.SearchAccount.AllowedAllDayBalTransfer, allowedAllDayBalTransfer)
                .AddData(DataKey.SearchAccount.IsAllDayBalTransfer, model.NonProduct.IsAllDayBalTransfer)
                .AddData(DataKey.SearchAccount.IsAllowAutoTransfer, model.NonProduct.IsAllowAutoTransfer)
                .AddData(DataKey.SearchAccount.ScheduleWeekDays, model.NonProduct.ScheduleWeekDays);

            request.SupportAutoTransfer = IsEnableAutoTransfer(message, CurrenciesOfferAutoTransfer);
            request.AutoTransferAffect = transferService.IsAutoTransferAffect(message.Currency, CurrenciesOfferAutoTransfer);

            return statusViewModelFactory.GetViewModels(request).Where(status => status.Visible).OrderBy(status => status.DisplayOrder);
        }

        internal IEnumerable<ISettingItemViewModel> BuildCustomerProductSettings(
            Products.Models.Customer actor,
            Products.Models.Customer searchAccount,
            CustomerStatusQueryBase message,
            CustomerStatusModel model)
        {
            var request = new SettingSearchAccountRequest(
                actor,
                searchAccount,
                message.Language,
                message.IsDarkMode,
                cacheRefService.GetProductGroupName,
                cacheRefService.GetProductName)
            {
                SubPageAccess = message.SubPageAccess
            };

            request
                 .AddData(DataKey.Actor.CanEditDailyTransferOption, model.NonProduct.TransferDaily)
                 .AddData(DataKey.Actor.HasUpdateAccountPermission, message.CanUpdateAccount)
                 .AddData(DataKey.SearchAccount.IsEnableSecurityCode, securityCodeChecker.IsEnableSecurityCode())
                 .AddData(DataKey.Theme.IsDarkMode, !string.IsNullOrWhiteSpace(message.DarkModeName));

            return settingViewModelFactory.GetViewModels(request).Where(setting => setting.Visible).OrderBy(setting => setting.DisplayOrder);
        }

        internal IEnumerable<ISettingItemMultipleViewModel> BuildCustomerProductMultipleSettings(
           Products.Models.Customer actor,
           Products.Models.Customer searchAccount,
           CustomerStatusMultipleQuery message,
           CustomerStatusModel model)
        {
            var request = new SettingSearchAccountRequest(
                actor,
                searchAccount,
                message.Language,
                message.IsDarkMode,
                cacheRefService.GetProductGroupName,
                cacheRefService.GetProductName)
            {
                SubPageAccess = message.SubPageAccess,
                SupportAutoTransfer = IsEnableAutoTransfer(message, CurrenciesOfferAutoTransfer),
                AutoTransferAffect = transferService.IsAutoTransferAffect(message.Currency, CurrenciesOfferAutoTransfer)
            };

            request
                 .AddData(DataKey.Actor.CanEditDailyTransferOption, model.NonProduct.TransferDaily)
                 .AddData(DataKey.Actor.HasUpdateAccountPermission, message.CanUpdateAccount)
                 .AddData(DataKey.SearchAccount.IsEnableSecurityCode, securityCodeChecker.IsEnableSecurityCode())
                 .AddData("customerIds", message.CustIds)
                 .AddData("usernames", message.Usernames)
                 .AddData("customerNames", message.Usernames)
                 .AddData("language", message.Language)
                 .AddData(DataKey.SearchAccount.IsOfferProtectedMode, message.IsOfferProtectedMode)
                 .AddData(DataKey.Theme.IsDarkMode, !string.IsNullOrWhiteSpace(message.DarkModeName));

            return settingMultipleViewModelFactory.GetViewModels(request).Where(setting => setting.Visible).OrderBy(setting => setting.DisplayOrder);
        }

        private bool IsEnableAutoTransfer(BaseQuery query, string offerCurrencies)
        {
            if (transferService.IsAllDayBalanceTransfer(query.ViewerId)
                || (query.IsSubAcc && !query.AllowTransfer))
            {
                return false;
            }

            return transferService.IsAutoTransferAffect(query.Currency, offerCurrencies);
        }
    }
}