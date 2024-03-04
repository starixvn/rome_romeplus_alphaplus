using Alpha.SecurityCode.Services;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.PartialUM.Configurations;
using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.RequestInfo;
using AlphaPlus.Settings.MemberInfo.API.Features.CustomerList;
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
using AlphaPlus.Settings.MemberInfo.API.Services.Transfer;
using AlphaPlus.Settings.Site.Features.CopyAccount;
using AlphaPlus.Settings.Site.Features.CreateAccount;
using AlphaPlus.Settings.Site.Features.DormantAccountList;
using AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold;
using AlphaPlus.Settings.Site.Features.QRCodeMemberSignIn;
using AlphaPlus.Settings.Site.Features.QRCodeMemberSignIn.Helpers;
using AlphaPlus.Settings.Site.Handlers;
using AlphaPlus.Settings.Site.SubAccount.Services;
using Fanex.Caching;
using Fanex.Cryptography.Aes;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestSharp;
using Sunplus.Agent.Utilities;

namespace AlphaPlus.Settings.Site.Helpers
{
    public static class ApplicationServicesHelper
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var customerSettingApiUrl = new Uri(configuration.GetValue<string>("CustomerSettingApi"));
            var qrCodeMemberLoginApi = new Uri(configuration.GetValue<string>("QRCodeMemberLogin:Api"));
            var qrCodeMemberLoginConfig = configuration.GetValue<string>("QRCodeMemberLogin:Config");

            services
                .AddFanexDataWithSqlServer(configuration)
                .AddFanexLogging(configuration)
                .AddRemotingRef(configuration)
                .AddHttpClient<ISubAccountService, SubAccountService>
                    (httpClient => httpClient.BaseAddress = new Uri(configuration.GetValue<string>("CustomerSettingApi")));

            services
                .AddSingleton<IAesService, AesService>()
                .AddSingleton<ISessionManager, SessionManager>()
                .AddScoped<Encryptor, EncryptorImpl>()
                .AddScoped<UsernameSuggestionService, UsernameSuggestionServiceImpl>()
                .AddHttpClient<ICopyAccountService, CopyAccountService>(httpClient => httpClient.BaseAddress = customerSettingApiUrl);

            services
                .AddHttpClient<ICreateAccountService, CreateAccountService>(httpClient => httpClient.BaseAddress = customerSettingApiUrl);

            services
                .AddSingleton<SecurityCodeChecker, SecurityCodeCheckerImpl>()
                .AddSingleton<Utilities.IP.IPService, Utilities.IP.IPServiceImpl>()
                .AddSingleton<IPService, IPServiceImpl>()
                .AddSingleton<RequestInfo.IPHelper, IPHelperImpl>()
                .AddSingleton<RequestInfo.IPHelper, IPHelperImpl>()
                .AddSingleton<IBrowserHelper, BrowserHelper>()
                .AddSingleton<IDeviceHelper, DeviceHelper>()
                .AddScoped<CustomerListService, CustomerListServiceImpl>()
                .AddScoped<TransferService, TransferServiceImpl>();

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
                .AddSingleton<QRCodeLoginConfigsHelper>((provider) => new QRCodeLoginConfigsHelperImpl(qrCodeMemberLoginConfig))
                .AddSingleton<IRestClient>((provider) => new RestClient(qrCodeMemberLoginApi))
                .AddSingleton<Alpha.CopyAccount.Services.ICopyAccountService, Alpha.CopyAccount.Services.CopyAccountService>()
                .AddScoped<QRCodeMemberSignInService, QRCodeMemberSignInServiceImpl>();

            services
                .AddSingleton<ProblemAccountWorkbook, ProblemAccountWorkbookImpl>()
                .AddScoped<IProblemAccountService, ProblemAccountService>();

            services
                .AddScoped<IndirectLoginAccountService, IndirectLoginAccountServiceImpl>();

            services
                .AddScoped<IWinlossService, WinlossService>()
                .AddScoped<CreditBalanceService, CreditBalanceServiceImpl>();

            services
                .AddSharedSession(configuration)
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