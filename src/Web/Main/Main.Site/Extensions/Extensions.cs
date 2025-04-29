using System.Net;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;
using AlphaPlus.Main.Site.Features.Account;
using AlphaPlus.Main.Site.Features.ApiKey;
using AlphaPlus.Main.Site.Features.Banner;
using AlphaPlus.Main.Site.Features.CustomerProfile;
using AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo;
using AlphaPlus.Main.Site.Features.Dashboard.Statistics;
using AlphaPlus.Main.Site.Features.Default.MenuBar;
using AlphaPlus.Main.Site.Features.Language;
using AlphaPlus.Main.Site.Features.Nickname;
using AlphaPlus.Main.Site.Features.NovaCasino;
using AlphaPlus.Main.Site.Features.OTP;
using AlphaPlus.Main.Site.Features.Password;
using AlphaPlus.Main.Site.Features.ProtectionCode;
using AlphaPlus.Main.Site.Features.SecurityCode;
using AlphaPlus.Main.Site.Features.Tour;
using AlphaPlus.Main.Site.Features.UMMessage;
using AlphaPlus.Main.Site.Features.WhiteLabel;
using AlphaPlus.Main.Site.Shared.Models;
using AlphaPlus.Main.Site.Shared.Services;
using AlphaPlus.RequestInfo;
using AlphaPlus.SecurityCode.Extensions;
using Fanex.Cryptography.Aes;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.RemotingRef;
using ONEbook.OTP.Helpers;
using ONEbook.OTP.Services;
using TelegramBot.Client.Services;

namespace AlphaPlus.Main.Site.Extensions;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR()
            .AddFanexData(configuration)
            .AddFanexLogging(configuration)
            .AddRemotingRef(configuration)
            .AddRequestInfo()
            .AddSecurityCodeServices(configuration);

        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton(Fanex.Logging.Logger.Log);

        services.AddAlphaPlusCore(configuration).AddSharedSession(configuration);

        services.AddSingleton<IMenuItemBuilder, MenuItemBuilder>();
        services.AddSingleton<IAesService, AesService>();
        services.AddSingleton<IUserSessionService, UserSessionService>();

        services.AddSingleton<ISiteMenuService, SiteMenuService>();
        services.AddScoped<ISessionManager, SessionManager>();
        services.AddSingleton<ISiteMapReader, SiteMapReader>();
        services.AddSingleton<IEncryption, Encryption>();
        services.AddSingleton<IBalanceInfoService, BalanceInfoService>();
        services.AddSingleton<IStatisticsService, StatisticsService>();

        services.AddTelegramBot(configuration);
        services.AddSingleton<ITransferService, TransferService>();
        services.AddSingleton<IUser, User>();

        services.AddSingleton<INicknameChecker, NicknameChecker>();
        services.AddSingleton<IBannerService, BannerService>();
        services.AddSingleton<IUMMessageService, UMMessageService>();
        services.AddSingleton<OtpService, OtpServiceImpl>();
        services.AddSingleton<OtpAdapter, OtpAdapterImpl>();
        services.AddSingleton<INetThrottlingService, NetThrottlingService>();

        services.AddSingleton<ICustomerService, CustomerService>();
        services.AddScoped<INovaCasinoService, NovaCasinoService>();

        services.AddHttpClient<IApiKeyService, ApiKeyService>(httpClient => httpClient.BaseAddress = new Uri(configuration["ApiKey:ServiceUrl"]));
        services.AddHttpClient<IAccountService, AccountService>(httpClient => httpClient.BaseAddress = new Uri(configuration.GetValue<string>("IdentityUrl")))
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });

        var customerServiceUri = new Uri(configuration.GetValue<string>("Fanex.Customer:ServiceUri"));
        services.AddHttpClient<IPasswordService, PasswordService>(httpClient => httpClient.BaseAddress = customerServiceUri);
        services.AddHttpClient<ISecurityCodeService, SecurityCodeService>(httpClient => httpClient.BaseAddress = customerServiceUri);
        services.AddHttpClient<ICustomerOtpService, CustomerOtpService>(httpClient => httpClient.BaseAddress = customerServiceUri);
        services.AddHttpClient<INicknameService, NicknameService>(httpClient => httpClient.BaseAddress = customerServiceUri);
        services.AddHttpClient<IProfilePictureService, ProfilePictureService>(httpClient => httpClient.BaseAddress = customerServiceUri);
        services.AddHttpClient<IProtectionCodeService, ProtectionCodeService>(httpClient => httpClient.BaseAddress = customerServiceUri);
        services.AddHttpClient<ILanguageService, LanguageService>(httpClient => httpClient.BaseAddress = customerServiceUri);
        services.AddHttpClient<IWhiteLabelService, WhiteLabelService>(HttpClient => HttpClient.BaseAddress = new Uri(configuration.GetValue<string>("WhiteLabel:BackOfficeApiUrl")));

        services.AddSingleton<IRewardTourService, RewardTourService>();
        services.AddHealthChecks();

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
