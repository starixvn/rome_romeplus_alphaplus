using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Settings.Site.Features.CopyAccount;
using AlphaPlus.Settings.Site.SubAccount.Services;
using Fanex.Cryptography.Aes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Sunplus.Agent.Utilities;

namespace AlphaPlus.Settings.Site.Helpers
{
    public static class ApplicationServicesHelper
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
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
                .AddHttpClient<ICopyAccountService, CopyAccountService>(httpClient => httpClient.BaseAddress = new Uri(configuration.GetValue<string>("CustomerSettingApi")));

            services
                .AddSharedSession(configuration)
                .AddHttpContextAccessor()
                .AddAuthorization()
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddSharedCookie();


            return services;
        }
    }
}