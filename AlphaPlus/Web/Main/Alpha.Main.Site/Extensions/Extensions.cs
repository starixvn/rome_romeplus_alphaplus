using Alpha.Main.Models;
using AlphaPlus.SecurityCode.Services;
using Fanex.Data.Repository;
using Fanex.RemotingRef;
using TelegramBot.Client.Services;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;
using AlphaPlus.RequestInfo;
using Fanex.Cryptography.Aes;
using Fanex.Caching;
using System.Net;
using AlphaPlus.Core.Extensions;
using AlphaPlus.SecurityCode.Configurations;
using Alpha.Main.Site.Features.Account;
using AlphaPlus.SecurityCode.Web;
using Alpha.Main.Site.Features.ApiKey;
using Alpha.Main.Site.Features.Banner;
using Alpha.Main.Site.Features.Theme;
using Alpha.Main.Site.Features.UMMessage;
using Alpha.Main.Site.Features.Default.MenuBar;
using Alpha.Main.Site.Features.Language;
using Fanex.Data;
using Alpha.Main.Site.Features.Dashboard.BalanceInfo;
using Alpha.Main.Site.Features.Dashboard.Statistics;
using Alpha.Main.Site.Features.Password;
using Alpha.Main.Site.Features.SecurityCode;
using Alpha.Main.Site.Features.Nickname;
using Alpha.Main.Site.Features.OTP;
using Alpha.Main.Site.Features.ProtectionCode;
using Alpha.Main.Site.Features.CustomerProfile;

namespace Alpha.Main.Site.Extensions;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR()
            .AddFanexData(configuration)
            .AddRemotingRef(configuration)
            .AddRequestInfo();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton(Fanex.Logging.Logger.Log);

        services.AddAlphaPlusCore(configuration).AddSharedSession(configuration);

        services.AddSingleton<ISecurityCodePathBuilder, SecurityCodePathBuilder>();
        services.AddSingleton<ISecurityCodeHttpContext, SecurityCodeHttpContext>();
        services.AddSingleton<IMenuItemBuilder, MenuItemBuilder>();
        services.AddSingleton<IAesService, AesService>();
        services.AddSingleton<IUserSessionService, UserSessionService>();

        services.AddSingleton<ISiteMenuService, SiteMenuService>();
        services.AddSingleton<IProfilePictureService, ProfilePictureService>();
        services.AddSingleton<ISessionManager, SessionManager>();
        services.AddSingleton<ISiteMapReader, SiteMapReader>();
        services.AddSingleton<IEncryption, Encryption>();
        services.AddSingleton<IBalanceInfoService, BalanceInfoService>();
        services.AddSingleton<IStatisticsService, StatisticsService>();
        services.AddSingleton<ISecurityCodeChecker, SecurityCodeChecker>();

        services.AddTelegramBot(configuration);
        services.AddSingleton<ITransferService, TransferService>();
        services.AddSingleton<IUser, User>();

        services.AddSingleton<INicknameService, NicknameService>();
        services.AddSingleton<INicknameChecker, NicknameChecker>();
        services.AddSingleton<IBannerService, BannerService>();
        services.AddSingleton<IUMMessageService, UMMessageService>();
        services.AddSingleton<IProtectionCodeService, ProtectionCodeService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<ILanguageService, LanguageService>();

        services.AddHttpClient<IApiKeyService, ApiKeyService>(httpClient => httpClient.BaseAddress = new Uri(configuration["ApiKey:ServiceUrl"]));
        services.AddHttpClient<IAccountService, AccountService>(httpClient => httpClient.BaseAddress = new Uri(configuration.GetValue<string>("IdentityUrl")))
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });


        var customerServiceUri = new Uri(configuration.GetValue<string>("Fanxex.Customer:ServiceUrl"));
        services.AddHttpClient<IPasswordService, PasswordService>(httpClient => httpClient.BaseAddress = customerServiceUri);
        services.AddHttpClient<ISecurityCodeService, SecurityCodeService>(httpClient => httpClient.BaseAddress = customerServiceUri);
        services.AddHttpClient<ICustomerOtpService, CustomerOtpService>(httpClient => httpClient.BaseAddress = customerServiceUri);
        services.Configure<SecurityCodeOptions>(configuration.GetSection(SecurityCodeOptions.SecurityCode));

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
        return services;
    }

    private static IServiceCollection AddRemotingRef(this IServiceCollection services, IConfiguration configuration)
    {
        RefDataTypeContainer
            .ConnectTo(new Uri(configuration["RemotingRefUrl"]))
            .FromApplication("AlphaPlus");

        services.AddSingleton<IRemotingRefService, RemotingRefService>();

        return services;
    }

    private static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceUri = new Uri(configuration["TelegramBot:ServiceUrl"]);
        services.AddHttpClient<ISettingService, SettingService>(httpClient => httpClient.BaseAddress = serviceUri);
        services.AddHttpClient<IBotService, BotService>(httpClient => httpClient.BaseAddress = serviceUri);
        services.AddHttpClient<INotificationService, NotificationService>(httpClient => httpClient.BaseAddress = serviceUri);
        services.AddHttpClient<IOTPService, OTPService>(httpClient => httpClient.BaseAddress = serviceUri);

        return services;
    }
}
