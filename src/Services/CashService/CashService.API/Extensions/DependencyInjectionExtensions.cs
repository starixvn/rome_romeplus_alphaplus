namespace AlphaPlus.CashService.API.Extensions;

using System.Net;
using AlphaPlus.CashService.API.Features.PendingWithdrawal.Services;
using AlphaPlus.CashService.API.Features.Shared.Constants;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Helpers;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.RemotingRef;
using Features.MemberSites.Services;
using Features.Payments.Services;
using Features.PendingDeposit.Services;
using Features.Promotion.Services;
using Features.Shared.Services;
using Features.Transactions.Services;
using AlphaPlus.CashService.API.Features.Notifications.Services;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFanexSql(configuration)
            .AddSingleton<IDynamicRepository, DynamicRepository>();

        services.AddFanexLogging(configuration);

        services.AddRemotingRef(configuration);

        services.AddScoped<ICommonService, CommonService>();
        services.AddScoped<IPaymentsService, PaymentsService>();
        services.AddScoped<IMemberSiteManagementService, MemberSiteManagementService>();
        services.AddScoped<IPendingDepositService, PendingDepositService>();
        services.AddScoped<IPendingWithdrawalService, PendingWithdrawalService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IPromotionService, PromotionService>();
        services.AddScoped<IBankService, BankService>();
        services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
        services.AddScoped<IPromotionReportService, PromotionReportService>();
        services.AddScoped<INotificationsService, NotificationsService>();

        return services;
    }

    private static void AddRemotingRef(this IServiceCollection services, IConfiguration configuration)
    {
        RefDataTypeContainer
            .ConnectTo(new Uri(configuration["RemotingRefUrl"]))
            .FromApplication("AlphaPlus");

        services.AddSingleton<IRemotingRefService, RemotingRefService>();
        services.AddSingleton<ICacheRefService, CacheRefService>();
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("ProxySettings").Get<ProxySettings>());

        return services;
    }

    public static IServiceCollection AddHttpClientService(this IServiceCollection services, IConfiguration configuration)
    {
        var proxySettings = configuration.GetSection("ProxySettings").Get<ProxySettings>();
        services.AddHttpClient(HttpClientName.ProxyHttpClient).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            UseProxy = proxySettings.IsUseProxy,
            Proxy = new WebProxy(proxySettings.ProxyAddress, proxySettings.BypassOnLocal)
        });

        services.AddSingleton(sp => configuration.GetSection("PaymentGatewayApiConfig").Get<PaymentGatewayApiConfig>());
        services.AddScoped<HttpClientHelper>();

        return services;
    }
}