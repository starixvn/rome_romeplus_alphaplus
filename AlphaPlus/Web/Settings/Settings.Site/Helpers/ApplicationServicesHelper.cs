using AlphaPlus.SecurityCode.Services;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.PartialUM.Configurations;
using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.RequestInfo;
using AlphaPlus.Settings.MemberInfo.API.Features.DormantAccountList;
using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyThresholds;
using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts;
using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.ReachedThreshold;
using AlphaPlus.Settings.MemberInfo.API.Features.ParlayMaxPayout;
using AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.API.Services.CreditBalance;
using AlphaPlus.Settings.MemberInfo.API.Services.IndirectLoginAccount;
using AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Settings.Site.Features.CopyAccount;
using AlphaPlus.Settings.Site.Features.CreateAccount;
using AlphaPlus.Settings.Site.Features.DormantAccountList;
using AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold;
using AlphaPlus.Settings.Site.Features.QRCodeMemberSignIn.Helpers;
using AlphaPlus.Settings.Site.Handlers;
using AlphaPlus.Settings.Site.SubAccount.Services;
using Fanex.Caching;
using Fanex.Cryptography.Aes;
using Fanex.Data;
using Fanex.RemotingRef;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestSharp;
using Sunplus.Agent.Utilities;
using MediatR;
using AlphaPlus.SecurityCode.Web;
using AlphaPlus.SecurityCode.Configurations;
using Alpha.CopyAccount.Authentications;
using AlphaPlus.Settings.ViewLogs.API.Services.Impl.Logs;
using AlphaPlus.Settings.ViewLogs.API.Services;
using AlphaPlus.Settings.ViewLogs.API.DataAccesses;
using AlphaPlus.Settings.ViewLogs.API.DataAccesses.Impl;
using AlphaPlus.Settings.ViewLogs.API.Services.Impl;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Helpers.Impl;
using AlphaPlus.Settings.Site.ViewLogs.Helpers;
using AlphaPlus.Settings.Site.Features.AccountInfo;
using AlphaPlus.Settings.Transfer.API.Services;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.NPOI;
using AlphaPlus.Settings.Transfer.API.Models.TransferToDownlineHistory;
using AlphaPlus.Products.ViewModels;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;
using AlphaPlus.Settings.Site.Features.CustomerList;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Products.Configurations;

namespace AlphaPlus.Settings.Site.Helpers
{
    public static class ApplicationServicesHelper
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var customerSettingApiUrl = new Uri(configuration.GetValue<string>("CustomerSettingApi"));

            services
                .AddMediatR(typeof(Program).Assembly)
                .AddFanexData(configuration)
                .AddFanexLogging(configuration)
                .AddRemotingRef(configuration)
                .AddRequestInfo()
                .AddAlphaPlusCore(configuration)
                .AddSharedSession(configuration)
                .AddHttpClient<ISubAccountService, SubAccountService>
                    (httpClient => httpClient.BaseAddress = customerSettingApiUrl);

            var qrCodeMemberLoginApi = new Uri(configuration.GetValue<string>("QRCodeMemberLogin:Api"));
            var qrCodeMemberLoginKey = configuration.GetValue<string>("QRCodeMemberLogin:Key");
            var qrCodeMemberLoginConfig = AppDomain.CurrentDomain.BaseDirectory + "/QRCodeMemberSiteLoginConfigs/QRCodeMemberSiteLoginConfig.json";

            services
                .AddHealthChecks();

            services
                .AddControllersWithViews()
                .AddNewtonsoftJson(option =>
                {
                    option.UseMemberCasing();
                })
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/{1}/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/{1}/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
                    options.ViewLocationFormats.Add("/MemberInfo/Views/Shared/{0}.cshtml");
                    options.ViewLocationFormats.Add("/MemberInfo/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/MemberInfo/Features/Shared/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/MemberInfo/Features/{1}/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/ViewLogs/Views/Shared/{0}.cshtml");
                    options.ViewLocationFormats.Add("/ViewLogs/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Transfer/Views/Shared/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Transfer/Views/{1}/{0}.cshtml");
                });

            services
                .AddSingleton<IAesService, AesService>()
                .AddSingleton<ISessionManager, SessionManager>()
                .AddScoped<Encryptor, EncryptorImpl>()
                .AddScoped<UsernameSuggestionService, UsernameSuggestionServiceImpl>()
                .AddHttpClient<ICopyAccountService, CopyAccountService>(httpClient => httpClient.BaseAddress = customerSettingApiUrl);

            services
                .AddHttpClient<ICreateAccountService, CreateAccountService>(httpClient => httpClient.BaseAddress = customerSettingApiUrl);

            services.AddSingleton<IRemotingRefService, RemotingRefService>();
            services.AddSingleton<GenericViewModelFactory<ISearchAccountInfoRequest, IStatusItemViewModel>, StatusViewModelFactory>();
            services.AddSingleton<GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemViewModel>, SettingViewModelFactory>();
            services.AddSingleton<GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemMultipleViewModel>, SettingMultipleViewModelFactory>();

            // SimpleServiceLocator.Instance
            //     .Register<IRemotingRefService>(new RemotingRefService())
            //     .Register<Func<string, string>>(ProductResources.ResourceManager.GetString)
            //     .Register<Func<string, string, string>>(Core.Helpers.MicroSites.GetMicroSiteUrl)
            //     .Register<Func<string, ResourceManager, string>>(ResourcesJsonConverter.ToConvert)
            //     .Register<GenericViewModelFactory<ISearchAccountInfoRequest, IStatusItemViewModel>>(new StatusViewModelFactory())
            //     .Register<GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemViewModel>>(new SettingViewModelFactory())
            //     .Register<GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemMultipleViewModel>>(new SettingMultipleViewModelFactory())
            //     .Register<QRCodeLoginConfigsHelper>(new QRCodeLoginConfigsHelperImpl(qrCodeMemberLoginConfig));

            services
                .AddSingleton<ISecurityCodePathBuilder, SecurityCodePathBuilder>()
                .AddSingleton<ISecurityCodeHttpContext, SecurityCodeHttpContext>()
                .AddSingleton<ISecurityCodeChecker, SecurityCodeChecker>()
                .Configure<SecurityCodeOptions>(configuration.GetSection(SecurityCodeOptions.SecurityCode))
                .AddSingleton<Utilities.IP.IPService, Utilities.IP.IPServiceImpl>()
                .AddScoped<IUser, Models.User>()
                .AddScoped<CustomerListService, CustomerListServiceImpl>()
                .AddScoped<Settings.MemberInfo.API.Services.Transfer.TransferService, Settings.MemberInfo.API.Services.Transfer.TransferServiceImpl>();

            services
                .AddSingleton<DormantAccountWorkbook, DormantAccountWorkbookImpl>()
                .AddScoped<DormantAccountService, DormantAccountServiceImpl>();

            services
                .Configure<PartialUMOptions>(configuration.GetSection(nameof(PartialUMOptions)))
                .AddSingleton(GetPartialUMConfig(configuration))
                .AddSingleton<ICacheService, CacheService>()
                .AddSingleton<IPartialUMDataAccess, PartialUMDataAccess>()
                .AddSingleton<IPartialUMService, PartialUMService>();

            services
                .AddSingleton<ReachedThresholdWorkbook, ReachedThresholdWorkbookImpl>()
                .AddScoped<ReachedThresholdService, ReachedThresholdServiceImpl>()
                .AddScoped<MyWatchlistAccountsService, MyWatchlistAccountsServiceImpl>()
                .AddScoped<MyThresholdsService, MyThresholdsServiceImpl>();

            services
                .AddSingleton<CacheRefService, CacheRefServiceImpl>()
                .AddSingleton<CheckUMService, CheckUMServiceImpl>()
                .AddScoped<ParlayMaxPayoutService, ParlayMaxPayoutServiceImpl>();

            services
                .AddSingleton<QrCodeAdapter, QrCodeAdapterImpl>()
                .AddScoped<PasswordService, PasswordServiceImpl>()
                .AddScoped<CheckBelongingService, CheckBelongingServiceImpl>()
                .AddSingleton<IRestClient>((provider) => new RestClient { Authenticator = new HmacAuthenticator(qrCodeMemberLoginKey), BaseUrl = qrCodeMemberLoginApi })
                .AddSingleton<Alpha.CopyAccount.Services.ICopyAccountService>((provider) => new Alpha.CopyAccount.Services.CopyAccountService(provider.GetRequiredService<IRestClient>()))
                .AddSingleton<QRCodeMemberSignInService, QRCodeMemberSignInServiceImpl>();

            services
                .AddSingleton<ProblemAccountWorkbook, ProblemAccountWorkbookImpl>()
                .AddScoped<IProblemAccountService, ProblemAccountService>();

            services
                .AddScoped<IndirectLoginAccountService, IndirectLoginAccountServiceImpl>();

            services
                .AddScoped<IWinlossService, WinlossService>()
                .AddScoped<CreditBalanceService, CreditBalanceServiceImpl>();

            services
                .AddScoped<WinLossLimitService, WinLossLimitServiceImpl>();

            services
                .AddSingleton<IDateTime, DateTimeWrap>()
                .AddScoped<TransformService, TransformServiceImpl>()
                .AddScoped<TrackingValueFormatter, TrackingValueFormatterImpl>()
                .AddScoped<ViewLogs.Helpers.MicroSites, MicroSitesImpl>()
                .AddScoped<LogFormatter, LogFormatterImpl>()
                .AddScoped<LogDataAccess, LogDataAccessImpl>()
                .AddScoped<ViewLogIPService, ViewLogIPServiceImpl>()
                .AddScoped<RemotingRefWrapper, RemotingRefWrapperImpl>()
                .AddScoped<Settings.ViewLogs.API.Services.CustomerService, CustomerServiceImpl>()
                .AddKeyedScoped<LogService, SettingServiceImpl>("settingLogSvc")
                .AddKeyedScoped<LogService, StatusServiceImpl>("statusLogSvc")
                .AddKeyedScoped<LogService, CreditServiceImpl>("creditLogSvc")
                .AddKeyedScoped<LogService, LoginServiceImpl>("loginLogSvc");

            services.AddSingleton<IAccountInfoService, AccountInfoService>();
            services
                .AddSingleton<Workbook<TransferToDownlineHistoryModel>, NpoiWorkbook<TransferToDownlineHistoryModel>>()
                .AddScoped<TransferService, TransferServiceImpl>()
                .AddScoped<AutoTransferService, AutoTransferServiceImpl>()
                .AddScoped<CashBalanceTransferService, CashBalanceTransferServiceImpl>()
                .AddScoped<MultipleTransferService, MultipleTransferServiceImpl>()
                .AddScoped<TransferAllAndClearCreditService, TransferAllAndClearCreditServiceImpl>()
                .AddScoped<TransferCacheService, TransferCacheServiceImpl>()
                .AddScoped<TransferToDownlineHistoryService, TransferToDownlineHistoryServiceImpl>();

            services
                .AddHttpContextAccessor()
                .AddAuthorization()
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddSharedCookie();

            return services;
        }

        private static Dictionary<string, IList<string>> GetPartialUMConfig(IConfiguration configuration)
        {
            var partialUMConfig = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
            var partialUMArray = configuration.GetSection("PartialUMOptions:PartialUM").Get<UMConfig[]>();

            foreach (var partialUM in partialUMArray)
            {
                foreach (var key in partialUM.SettingKeys)
                {
                    if (!partialUMConfig.TryGetValue(key, out var functionNames))
                    {
                        functionNames = new List<string>();
                        partialUMConfig[key] = functionNames;
                    }

                    functionNames.Add(partialUM.FunctionName);
                }
            }

            return partialUMConfig;
        }
    }
}