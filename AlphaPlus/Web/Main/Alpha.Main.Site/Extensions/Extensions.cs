using Alpha.Main.Helpers;
using Alpha.Main.Helpers.Impl;
using Alpha.Main.Models;
using Alpha.Main.Models.Menus;
using Alpha.Main.ProfilePicture.Services;
using Alpha.Main.Services;
using Alpha.Main.Services.Impl;
using AlphaPlus.SecurityCode.Services;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.RemotingRef;
using ONEbook.OTP.Helpers;
using ONEbook.OTP.Services;
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

namespace Alpha.Main.Site.Extensions;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR()
            .AddFanexSql(configuration)
            .AddRemotingRef(configuration)
            .AddRequestInfo();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton(Fanex.Logging.Logger.Log);

        services.AddAlphaPlusCore(configuration);

        services.AddSingleton<ISecurityCodePathBuilder, SecurityCodePathBuilder>();
        services.AddSingleton<ISecurityCodeHttpContext, SecurityCodeHttpContext>();
        services.AddSingleton<MenuItemBuilder, MenuItemBuilderImpl>();
        services.AddSingleton<CacheRefService, CacheRefServiceImpl>();
        services.AddSingleton<IAesService, AesService>();
        services.AddSingleton<IUserSessionService, UserSessionService>();

        services.AddSingleton<SiteMenuService, SiteMenuServiceImpl>();
        services.AddSingleton<ProfilePictureServices, ProfilePictureServicesImpl>();
        services.AddSingleton<SecurityCodeService, SecurityCodeServiceImpl>();
        services.AddSingleton<ISessionManager, SessionManager>();
        services.AddSingleton<PasswordService, PasswordServiceImpl>();
        services.AddSingleton<SiteMapReader, SiteMapReaderImpl>();
        services.AddSingleton<IEncryption, Encryption>();
        services.AddSingleton<BalanceUpDayInfoService, BalanceUpDayInfoServiceImpl>();
        services.AddSingleton<BalanceInfoService, BalanceInfoServiceImpl>();
        services.AddSingleton<StatisticsService, StatisticsServiceImpl>();
        services.AddSingleton<ISecurityCodeChecker, SecurityCodeChecker>();


        services.AddSingleton<OtpService, OtpServiceImpl>();
        services.AddSingleton<OtpAdapter, OtpAdapterImpl>();
        services.AddSingleton<QrCodeAdapter, QrCodeAdapterImpl>();
        services.AddSingleton<RandomHelper, RandomHelperImpl>();
        services.AddSingleton<JsonHelper, JsonHelperImpl>();
        services.AddSingleton<CustomerOtpService, CustomerOtpServiceImpl>();

        services.AddTelegramBot(configuration);
        services.AddSingleton<TelegramBotService, TelegramBotServiceImpl>();
        services.AddSingleton<TransferService, TransferServiceImpl>();
        services.AddSingleton<IUser, User>();

        services.AddSingleton<LogoutService, LogoutServiceImpl>();

        services.AddSingleton<NicknameService, NicknameServiceImpl>();

        services.AddSingleton<INicknameChecker, NicknameChecker>();

        services.AddSingleton<KickoutCustomerService, KickoutCustomerServiceImpl>();
        services.AddSingleton<IBannerService, BannerService>();
        services.AddSingleton<IUMMessageService, UMMessageService>();
        services.AddSingleton<ProtectionCodeService, ProtectionCodeServiceImpl>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<UpdateUsageLanguageService, UpdateUsageLanguageServiceImpl>();

        services.AddHttpClient<IApiKeyService, ApiKeyService>(httpClient => httpClient.BaseAddress = new Uri(configuration["ApiKey:ServiceUrl"]));
        services.AddHttpClient<IAccountService, AccountService>(httpClient => httpClient.BaseAddress = new Uri(configuration.GetValue<string>("IdentityUrl")))
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });

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
