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
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.Helpers;
using AlphaPlus.Settings.Site.Features.MemberInfo.Handlers;
using AlphaPlus.Settings.Site.Features.SubAccount.Services;
using Fanex.Cryptography.Aes;
using Fanex.Data;
using Fanex.RemotingRef;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestSharp;
using MediatR;
using Alpha.CopyAccount.Authentications;
using AlphaPlus.Settings.ViewLogs.API.Services.Impl.Logs;
using AlphaPlus.Settings.ViewLogs.API.Services;
using AlphaPlus.Settings.ViewLogs.API.DataAccesses;
using AlphaPlus.Settings.ViewLogs.API.DataAccesses.Impl;
using AlphaPlus.Settings.ViewLogs.API.Services.Impl;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Helpers.Impl;
using AlphaPlus.Settings.Site.Features.ViewLogs.Helpers;
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
using AlphaPlus.Settings.Site.Features.Shared.Services;
using Fanex.Caching;
using AlphaPlus.Settings.Site.Features.ProductStatus;
using Fanex.Data.Repository;
using AlphaPlus.Core.Models;
using Fanex.Customer.Client.Configurations;
using AlphaPlus.Core.Filters;
using AlphaPlus.SecurityCode.Filters;
using AlphaPlus.Settings.Site.Features.UserNameSuggestion;
using AlphaPlus.Core.Utils;
using AlphaPlus.Settings.Site.Features.PositionTaking.Services;
using AlphaPlus.Settings.MemberInfo.API.Features.Commission;
using AlphaPlus.SecurityCode.Extensions;

namespace AlphaPlus.Settings.Site.Features.Helpers
{
    public static class ApplicationServicesHelper
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var customerSettingApiUrl = new Uri(configuration.GetValue<string>("Fanex.Customer:ServiceUri"));

            services
                .AddMediatR(typeof(Program).Assembly)
                .AddFanexData(configuration)
                .AddFanexLogging(configuration)
                .AddRemotingRef(configuration)
                .AddRequestInfo()
                .AddAlphaPlusCore(configuration)
                .AddSharedSession(configuration)
                .AddSingleton<IEncryption, Core.Utils.Encryption>()
                .AddHttpClient<ISubAccountService, SubAccountService>
                    (httpClient => httpClient.BaseAddress = customerSettingApiUrl);

            var qrCodeMemberLoginApi = new Uri(configuration.GetValue<string>("QRCodeMemberLogin:Api"));
            var qrCodeMemberLoginKey = configuration.GetValue<string>("QRCodeMemberLogin:Key");
            var qrCodeMemberLoginConfig = AppDomain.CurrentDomain.BaseDirectory + "/Features/QRCodeMemberSiteLoginConfigs/QRCodeMemberSiteLoginConfig.json";

            services
                .AddHealthChecks();

            services
                .AddControllersWithViews(options =>
                {
                    options.Filters.Add<CheckBelongingFilterAttribute>();
                    options.Filters.Add<AfterLoginFlowFilterAttribute>();
                    options.Filters.Add<ProtectedModeFilterAttribute>();
                    options.Filters.Add<SecurityCodeFilterAttribute>();
                })
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
                    options.ViewLocationFormats.Add("/Features/MemberInfo/Views/Shared/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/MemberInfo/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/MemberInfo/Features/Shared/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/MemberInfo/Features/{1}/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/ViewLogs/Views/Shared/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/ViewLogs/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/Transfer/Views/Shared/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/Transfer/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/{1}/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/PositionTaking/Views/{1}/{0}.cshtml");
                });

            services
                .AddSingleton<IAesService, AesService>()
                .AddSingleton<ISessionManager, SessionManager>()
                .AddSingleton<IEncryption, Encryption>()
                .AddHttpClient<UsernameSuggestionService, UsernameSuggestionServiceImpl>(httpClient => httpClient.BaseAddress = customerSettingApiUrl);


            services.AddSingleton<IRemotingRefService, RemotingRefService>();
            services.AddSingleton<GenericViewModelFactory<ISearchAccountInfoRequest, IStatusItemViewModel>, StatusViewModelFactory>();
            services.AddSingleton<GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemViewModel>, SettingViewModelFactory>();
            services.AddSingleton<GenericViewModelFactory<ISearchAccountInfoRequest, ISettingItemMultipleViewModel>, SettingMultipleViewModelFactory>();

            services.AddSingleton<ICacheRefService, Features.Shared.Services.CacheRefService>()
                .AddSingleton<IProductStatusService>((provider) => new ProductStatusService(
                    provider.GetRequiredService<Fanex.Logging.ILogger>(),
                    provider.GetRequiredService<IDynamicRepository>(),
                    provider.GetRequiredService<ICacheRefService>(),
                    UserPermission.HasPermissionOnProduct));

            services.AddSingleton(new CustomerClientConfiguration().Initialize(configuration));
            services.AddSingleton<Features.Shared.Models.IUser, Features.Shared.Models.User>();
            services.AddSingleton<Fanex.Customer.Client.Services.ICustomerService, Fanex.Customer.Client.Services.CustomerService>();

            services
                .AddSecurityCodeServices(configuration)
                .AddSingleton<Utilities.IP.IPService, Utilities.IP.IPServiceImpl>()
                .AddScoped<IUser, Features.MemberInfo.Models.User>()
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
                .AddSingleton<Settings.MemberInfo.API.Services.CacheRefService, CacheRefServiceImpl>()
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
                .AddSingleton<IProductService, ProductService>()
                .AddSingleton<IAccountService, AccountService>()
                .AddSingleton<ICommissionService, CommissionService>()
                .AddSingleton<WinLossLimitService, WinLossLimitServiceImpl>();

            services
                .AddSingleton<IDateTime, DateTimeWrap>()
                .AddScoped<TransformService, TransformServiceImpl>()
                .AddScoped<TrackingValueFormatter, TrackingValueFormatterImpl>()
                .AddScoped<Features.ViewLogs.Helpers.MicroSites, MicroSitesImpl>()
                .AddScoped<LogFormatter, LogFormatterImpl>()
                .AddScoped<LogDataAccess, LogDataAccessImpl>()
                .AddScoped<ViewLogIPService, ViewLogIPServiceImpl>()
                .AddScoped<RemotingRefWrapper, RemotingRefWrapperImpl>()
                .AddScoped<Settings.ViewLogs.API.Services.CustomerService, CustomerServiceImpl>()
                .AddKeyedScoped<LogService, SettingServiceImpl>("settingLogSvc")
                .AddKeyedScoped<LogService, StatusServiceImpl>("statusLogSvc")
                .AddKeyedScoped<LogService, CreditServiceImpl>("creditLogSvc")
                .AddKeyedScoped<LogService, LoginServiceImpl>("loginLogSvc");

            services
                .AddSingleton<Workbook<TransferToDownlineHistoryModel>, NpoiWorkbook<TransferToDownlineHistoryModel>>()
                .AddScoped<ITransferService, TransferService>()
                .AddScoped<AutoTransferService, AutoTransferServiceImpl>()
                .AddScoped<ICashBalanceTransferService, CashBalanceTransferService>()
                .AddScoped<ITransferListService, TransferListService>()
                .AddScoped<IYesterdayBalanceTransferService, YesterdayBalanceTransferService>()
                .AddScoped<TransferAllAndClearCreditService, TransferAllAndClearCreditServiceImpl>()
                .AddScoped<TransferToDownlineHistoryService, TransferToDownlineHistoryServiceImpl>();

            services
                .AddHttpClient<IPositionTakingService, PositionTakingService>(httpClient => httpClient.BaseAddress = customerSettingApiUrl);

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