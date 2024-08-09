using AlphaPlus.Core;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using Fanex.Cryptography.Aes;
using Fanex.Customer.Client.Services;
using Fanex.Customer.Client.Configurations;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.Customer.Requests;
using AlphaPlus.Settings.Site.Features.Customer.ViewModels;
using AlphaPlus.Core.Configurations;
using System.Text.Json;
using Fanex.Customer.Client.Params;
using Fanex.Customer.Client.Constants;
using AlphaPlus.Shared.AppResources.Settings;
using AlphaPlus.Core.Utils;

namespace AlphaPlus.Settings.Site.Features.Customer
{
    public class CustomerController : BaseController
    {
        private const string DCSStaticSessionKey = "DCSStaticAppPathSessionKey";
        private const string HashKeySessionKey = "Alpha.MemberInfo.Site.Controllers.CustomerController.PasswordHash";
        private readonly int systemId;
        private readonly IEncryption encryption;
        private readonly ISessionManager sessionManager;
        private readonly IAesService aesService;
        private readonly ICustomerService customerService;

        public CustomerController(
            IEncryption encryption,
            ISessionManager sessionManager,
            IAesService aesService,
            CustomerClientConfiguration clientConfiguration,
            ICustomerService customerService)
        {
            this.encryption = encryption;
            this.sessionManager = sessionManager;
            this.aesService = aesService;
            systemId = clientConfiguration.SystemId;
            this.customerService = customerService;
        }

        private string CDNUri => sessionManager.Get<string>(DCSStaticSessionKey) ?? Core.Configurations.ConfigurationManager.GetValue<string>("Fanex.Customer:CdnUri");

        public async Task<ActionResult> Create(CreateCustomerQuery query)
        {
            var limitedRulesResult = await customerService.CheckAllLimitedRules(query.ViewerId, query.ViewerLevelId, query.ViewerUsername, query.Language);

            var viewModel = limitedRulesResult.IsFailed
                ? new CreateCustomerViewModel(query, true, limitedRulesResult.Message)
                : new CreateCustomerViewModel(
                query,
                new AppConfiguration(GetApplicationPath(), systemId),
                CDNUri,
                GetHashKey(),
                await customerService.GetSuggestUsername(query.CustName, query.ViewerLevelId));

            return View(viewModel);
        }

        public ActionResult EditCustomer(EditCustomerQuery query)
        {
            var viewModel = new EditCustomerViewModel(
                query,
                new AppConfiguration(GetApplicationPath(), systemId),
                CDNUri);

            return View(viewModel);
        }

        public ActionResult EditMultiple(EditMultipleCustomersQuery query)
        {
            var viewModel = new EditMultipleCustomersViewModel(
                query,
                new AppConfiguration(GetApplicationPath(), systemId),
                CDNUri);

            return View(viewModel);
        }

        public ActionResult SportBookInformation(SportBookInformationQuery query)
        {
            var viewModel = new SportBookInformationViewModel(
                query,
                new AppConfiguration(GetApplicationPath(), systemId),
                CDNUri
            );

            return View(viewModel);
        }

        private string GetHashKey()
        {
            var hashKey = encryption.RandomString(4);
            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromBody] CreateCustomerRequest request)
        {
            if (request.IsInternal || !request.IsValid())
            {
                return UnAuthorized();
            }

            var generalInfo = request.GeneralInfo;
            var rawPassword = aesService.Decrypt(generalInfo.Password, sessionManager.Get<string>(HashKeySessionKey));
            generalInfo.Password = encryption.CfsEnCode(rawPassword);
            generalInfo.UpLineRoleId = request.ViewerLevelId;
            generalInfo.Site = request.Site;
            generalInfo.SiteLabel = AppSettings.SiteLabel;

            var response = await customerService.CreateCustomer(new CreateCustomerParam(
                generalInfo,
                request.ProductGroups,
                request.ViewerId,
                request.SubAccId,
                request.IP,
                request.Site,
                AppSettings.SiteLabel,
                CreationModes.FullMode,
                request.Language));

            return Content(response);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Update([FromBody] UpdateCustomerRequest request)
        {
            if (request.IsInternal)
            {
                return UnAuthorized();
            }

            var response = await customerService.UpdateCustomer(
                new UpdateCustomerParam(
                    request.GeneralInfo,
                    request.ProductGroups,
                    request.ViewerId,
                    request.SubAccId,
                    request.CustId,
                    request.CustName,
                    request.RoleId,
                    request.UplineRoleId,
                    request.IP,
                    request.Site,
                    AppSettings.SiteLabel,
                    request.Language));

            return Content(response);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateMultipleCustomers([FromBody] UpdateMultipleCustomerRequest request)
        {
            if (request.IsInternal)
            {
                return UnAuthorized();
            }

            var response = await customerService.UpdateMultipleCustomers(
                new UpdateMultipleCustomerParam(
                    request.ProductGroups,
                    request.ViewerId,
                    request.SubAccId,
                    request.CustId,
                    request.CustName,
                    request.RoleId,
                    request.UplineRoleId,
                    request.IP,
                    request.CustomerIds,
                    request.CustomerNames,
                    request.Site,
                    AppSettings.SiteLabel,
                    request.Language));

            return Content(response);
        }

        public async Task<ActionResult> GetCustomerSetting(GetCustomerSettingRequest request)
            => Content(await customerService.GetCustomerSetting(request.CustId, request.RoleId, systemId, request.UplineRoleId, request.Language, request.IsGetUpLine, request.ProductGroupId, request.SiteLabel));

        public async Task<ActionResult> GetCurrentWinLossAmount(CustomerWinLossAmountQuery request)
         => Content(await customerService.GetCustomerCurrentWinLossAmounts(request.CustId, request.SportType, request.BetType, request.Language));

        public async Task<ActionResult> GetPTSettingInformation(SportBookInformationQuery request)
            => Content(await customerService.GetPTSettingInformation(request.CustId, AppSettings.LocalTest, request.Site, request.Language));

        public async Task<ActionResult> GetMaxPayoutSettingInformation(SportBookInformationQuery request)
            => Content(await customerService.GetMaxPayoutSettingInformation(request.Site, request.SiteLabel, request.Language));

        public async Task<ActionResult> GetProductCategories(SportBookInformationQuery request)
            => Content(await customerService.GetProductCategories(request.CustId, request.Site, AppSettings.SiteLabel, request.CurrencyId, request.Language));

        public async Task<ActionResult> GetCommissionHint(SportBookInformationQuery request)
            => Content(await customerService.GetCommissionHint(request.CustId, request.Site, request.Language));

        public async Task<ActionResult> CheckExistedCustomer(string username)
           => Json(await customerService.CheckExistedCustomer(username));

        public async Task<IActionResult> UpdateInfo(int custId)
            => View(await customerService.GetCustomerInfo(custId));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateCustomerInfoRequest request)
            => Json(await customerService.UpdateCustomerInfo(new UpdateCustomerInfoParam
            {
                CustId = request.CustId,
                FirstName = request.FirstName?.Trim() ?? string.Empty,
                LastName = request.LastName?.Trim() ?? string.Empty,
                Phone = request.Phone?.Trim() ?? string.Empty,
                MobilePhone = request.MobilePhone?.Trim() ?? string.Empty,
                Email = request.Email?.Trim() ?? string.Empty,
                Fax = request.Fax?.Trim() ?? string.Empty,
                UserId = request.ViewerId,
                SubAccId = request.SubAccId,
                IP = request.IP,
                Language = request.LangKey
            }));

        private string GetApplicationPath()
        {
            var applicationPath = HttpContext.Request.PathBase.Value;
            if (!applicationPath.EndsWith('/'))
            {
                applicationPath += "/";
            }

            return applicationPath;
        }

        private ContentResult UnAuthorized() => Content(JsonSerializer.Serialize(Result.CreateErrorResult(Resources.NotAuthorized)));
    }
}