using System.Text.Json;
using AlphaPlus.Core;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using AlphaPlus.Products.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.Site.Features.Customer.Requests;
using AlphaPlus.Settings.Site.Features.Customer.ViewModels;
using AlphaPlus.Settings.Site.Features.ProductStatus;
using AlphaPlus.Shared.AppResources.Settings;
using Fanex.Cryptography.Aes;
using Fanex.Customer.Client.Configurations;
using Fanex.Customer.Client.Constants;
using Fanex.Customer.Client.Params;
using Fanex.Customer.Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.Site.Features.Customer
{
    public class CustomerController : BaseController
    {
        private const string DCSStaticSessionKey = "DCSStaticAppPathSessionKey";
        private const string HashKeySessionKey = "Alpha.MemberInfo.Site.Controllers.CustomerController.PasswordHash";
        private const string AssetsVersionCacheKey = "Customer:AssetsVersion";
        private readonly int systemId;
        private readonly IEncryption encryption;
        private readonly ISessionManager sessionManager;
        private readonly IAesService aesService;
        private readonly ICustomerService customerService;
        private readonly IProductStatusService productStatusService;
        private readonly IUserSession userSession;
        private readonly IMemoryCache memoryCache;

        private readonly JsonSerializerOptions serializeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public CustomerController(
            IEncryption encryption,
            ISessionManager sessionManager,
            IAesService aesService,
            CustomerClientConfiguration clientConfiguration,
            ICustomerService customerService,
            IProductStatusService productStatusService,
            IUserSession userSession,
            IMemoryCache memoryCache)
        {
            this.encryption = encryption;
            this.sessionManager = sessionManager;
            this.aesService = aesService;
            systemId = clientConfiguration.SystemId;
            this.customerService = customerService;
            this.productStatusService = productStatusService;
            this.userSession = userSession;
            this.memoryCache = memoryCache;
        }

        private string GetAssetsVersion()
        {
            var version = memoryCache.Get(AssetsVersionCacheKey) as string;
            if (string.IsNullOrWhiteSpace(version))
            {
                version = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                memoryCache.Set(AssetsVersionCacheKey, version, DateTime.Now.AddHours(4));
            }
            return version;
        }

        private string CDNUri => sessionManager.Get<string>(DCSStaticSessionKey) ?? Core.Configurations.ConfigurationManager.GetValue<string>("Fanex.Customer:CdnUri");

        public async Task<ActionResult> CreateDirectMember(CreateCustomerQuery query)
        {
            var limitedRulesResult = await customerService.CheckAllLimitedRules(query.ViewerId, query.ViewerLevelId, query.ActorUsername, true, query.Language);

            var viewModel = limitedRulesResult.IsFailed
                ? new CreateDirectMemberViewModel(query, true, limitedRulesResult.Message)
                : new CreateDirectMemberViewModel(
                query,
                new AppConfiguration(GetApplicationPath(), systemId),
                CDNUri,
                GetAssetsVersion(),
                GetHashKey(),
                await customerService.GetSuggestDirectMemberUsername(query.CustName));

            return View(viewModel);
        }

        public async Task<ActionResult> CreateCashMember(CreateCustomerQuery query)
        {
            var limitedRulesResult = await customerService.CheckAllLimitedRules(query.ViewerId, query.ViewerLevelId, query.ActorUsername, false, query.Language);

            var viewModel = limitedRulesResult.IsFailed
                ? new CreateCustomerViewModel(query, true, limitedRulesResult.Message)
                : new CreateCustomerViewModel(
                query,
                new AppConfiguration(GetApplicationPath(), systemId),
                CDNUri,
                GetAssetsVersion(),
                GetHashKey(),
                await customerService.GetSuggestUsername(query.CustName, query.ViewerLevelId),
                productGroupId: 0);

            return View(viewModel);
        }

        public async Task<ActionResult> Create(CreateCustomerQuery query)
        {
            var limitedRulesResult = await customerService.CheckAllLimitedRules(query.ViewerId, query.ViewerLevelId, query.ActorUsername, false, query.Language);

            var viewModel = limitedRulesResult.IsFailed
                ? new CreateCustomerViewModel(query, true, limitedRulesResult.Message)
                : new CreateCustomerViewModel(
                query,
                new AppConfiguration(GetApplicationPath(), systemId),
                CDNUri,
                GetAssetsVersion(),
                GetHashKey(),
                await customerService.GetSuggestUsername(query.CustName, query.ViewerLevelId));

            return View(viewModel);
        }

        public ActionResult EditCustomer(EditCustomerQuery query)
        {
            var viewModel = new EditCustomerViewModel(
                query,
                new AppConfiguration(GetApplicationPath(), systemId),
                CDNUri,
                GetAssetsVersion());

            return View(viewModel);
        }

        public async Task<ActionResult> AccountInfo(int CustId)
        {
            var accountInfo = await customerService.GetAccountInfo(CustId);
            return View(new AccountInfoViewModel
            {
                Credit = accountInfo.MaxCredit,
                Usernames = accountInfo.UserName,
                Domains = accountInfo.domains
            });
        }

        public ActionResult EditMultiple(EditMultipleCustomersQuery query)
        {
            var viewModel = new EditMultipleCustomersViewModel(
                query,
                new AppConfiguration(GetApplicationPath(), systemId),
                CDNUri,
                GetAssetsVersion());

            return View(viewModel);
        }

        public ActionResult SportBookInformation(SportBookInformationQuery query)
        {
            var viewModel = new SportBookInformationViewModel(
                query,
                new AppConfiguration(GetApplicationPath(), systemId),
                CDNUri,
                GetAssetsVersion()
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
            generalInfo.Password = rawPassword;
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
                request.CustDBGroupId,
                request.Group,
                request.NumberOfAccounts,
                CreationModes.FullMode,
                request.Language));

            return Content(response);
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCashCustomer([FromBody] CreateCustomerRequest request)
        {
            if (request.IsInternal || !request.IsValid())
            {
                return UnAuthorized();
            }

            var generalInfo = request.GeneralInfo;
            var rawPassword = aesService.Decrypt(generalInfo.Password, sessionManager.Get<string>(HashKeySessionKey));
            generalInfo.Password = rawPassword;
            generalInfo.UpLineRoleId = request.ViewerLevelId;
            generalInfo.Site = request.Site;
            generalInfo.SiteLabel = AppSettings.SiteLabel;

            var response = await customerService.CreateCashCustomer(new CreateCustomerParam(
                generalInfo,
                request.ProductGroups,
                request.ViewerId,
                request.SubAccId,
                request.IP,
                request.Site,
                AppSettings.SiteLabel,
                request.CustDBGroupId,
                request.Group,
                request.NumberOfAccounts,
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
                    request.CustDBGroupId,
                    request.Site,
                    AppSettings.SiteLabel,
                    request.Group,
                    request.AgentId,
                    request.Language));

            return Content(response);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateMultipleCustomers(UpdateMultipleCustomerRequest request)
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
                    request.CustDBGroupId,
                    request.CustomerIds,
                    request.CustomerNames,
                    request.Site,
                    AppSettings.SiteLabel,
                    request.Group,
                    request.AgentId,
                    request.Language));

            return Content(response);
        }

        public async Task<ActionResult> SearchSuggestDirectMembers(DirectMemberSuggestionQuery request)
            => Json(await customerService.SearchSuggestDirectMembers(request.ViewerId, request.SearchUsername));
      
        public async Task<ActionResult> SearchSuggestCashMembers(DirectMemberSuggestionQuery request)
            => Json(await customerService.SearchSuggestCashMembers(request.ViewerId, request.SearchUsername));

        public async Task<int> CheckDirectMemberUserName(DirectMemberSuggestionQuery request)
            => await customerService.CheckDirectMemberUserName(request.ViewerId, request.SearchUsername);    
        
        public async Task<int> CheckCashMemberUserName(DirectMemberSuggestionQuery request)
            => await customerService.CheckCashMemberUserName(request.ViewerId, request.SearchUsername);

        public async Task<ActionResult> GetCustomerSetting(GetCustomerSettingRequest request)
            => Content(await customerService.GetCustomerSetting(
                request.CustId,
                request.RoleId,
                systemId,
                request.UplineRoleId,
                request.Language,
                request.IsGetUpLine,
                request.ProductGroupId,
                request.SiteLabel,
                agentId: request.AgentId)); 
        
        public async Task<ActionResult> GetCashCustomerSetting(GetCustomerSettingRequest request)
            => Content(await customerService.GetCashCustomerSetting(
                request.CustId,
                request.RoleId,
                systemId,
                request.UplineRoleId,
                request.Language,
                request.IsGetUpLine,
                request.ProductGroupId,
                request.SiteLabel));
        

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
        public async Task<IActionResult> UpdateInfoPost(UpdateCustomerInfoRequest request)
        {
            if (request.IsInternal)
            {
                return Json(Result.CreateErrorResult(Resources.NotAuthorized));
            }

            return Json(await customerService.UpdateCustomerInfo(new UpdateCustomerInfoParam
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
        }

        [HttpGet]
        public IActionResult RedirectPT(int custId)
        {
            string urlMainPage = string.Empty;

            var session = userSession;
            var isDarkMode = !string.IsNullOrWhiteSpace(session.DarkModeName);

            int customerLevel;

            if (session.IsSuper)
            {
                customerLevel = 3;
            }
            else if (session.IsMaster)
            {
                customerLevel = 2;
            }
            else
            {
                customerLevel = 1;
            }

            var productPTTabs = InitProductGroupTabs(custId, customerLevel, isDarkMode);
            productPTTabs.Add(BuildAccountInfoTab(custId, customerLevel, isDarkMode));

            foreach (var product in productPTTabs)
            {
                if (product.Enabled)
                {
                    urlMainPage = product.URL;
                    break;
                }
            }

            return View(new RedirectPTViewModel(urlMainPage, productPTTabs));
        }

        private string GetApplicationPath()
        {
            var applicationPath = HttpContext.Request.PathBase.Value;
            if (!applicationPath.EndsWith('/'))
            {
                applicationPath += "/";
            }

            return applicationPath;
        }

        private ContentResult UnAuthorized() => Content(JsonSerializer.Serialize(Result.CreateErrorResult(Resources.NotAuthorized), serializeOptions));

        private SettingProductTab BuildAccountInfoTab(int customerId, int customerLevel, bool isDarkMode)
        {
            return new SettingProductTab
            {
                Enabled = true,
                Id = "AccountInfo",
                Name = AlphaPlus.Shared.AppResources.Main.Resources.accountInfo,
                URL = MicroSites.SettingUrl(
                    string.Format("~/Customer/AccountInfo?CustId={0}&roleId={1}&&lng={2}&isDarkMode={3}",
                    customerId,
                    customerLevel,
                    userSession.Language,
                    isDarkMode.ToString().ToLowerInvariant())),
            };
        }
        private IList<SettingProductTab> InitProductGroupTabs(int customerId, int customerLevel, bool isDarkMode)
        {
            IList<SettingProductTab> productPTTabs = [];
            var offeredProductGroups = productStatusService.GetOfferedProductGroup(userSession.CustId, userSession.RoleId);
            if (offeredProductGroups != null && offeredProductGroups.Any())
            {
                foreach (var productGroup in offeredProductGroups.Where(x => x.Id != ProductGroupConstants.Sportsbook))
                {
                    productPTTabs.Add(new SettingProductTab
                    {
                        URL = BuildProductGroupSettingUrl(customerId, customerLevel, productGroup.Id, isDarkMode),
                        Enabled = true,
                        Name = productGroup.Name
                    });
                }
            }

            return productPTTabs;
        }

        private string BuildProductGroupSettingUrl(int customerId, int customerLevel, int productGroupId, bool isDarkMode)
        {
            return MicroSites.SettingUrl(
                string.Format("~/Customer/EditCustomer?CustId={0}&roleId={1}&productGroupId={2}&lng={3}&isDarkMode={4}",
                customerId,
                customerLevel,
                productGroupId,
                userSession.Language,
                isDarkMode.ToString().ToLowerInvariant()));
        }
    }
}