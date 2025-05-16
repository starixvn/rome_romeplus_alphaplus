using AlphaPlus.CashService.API.Features.Payments.Services;
using AlphaPlus.CashService.API.Features.MemberSites.Services;
using AlphaPlus.CashService.API.Features.Shared.Services;
using AlphaPlus.CashService.API.Services;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.RemotingRef;

namespace AlphaPlus.CashService.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFanexSql(configuration)
            .AddSingleton<IDynamicRepository, DynamicRepository>();
            
        FanexLoggingExtensions.AddFanexLogging(services, configuration);

        services.AddRemotingRef(configuration);

        services.AddScoped<ICommonService, CommonService>();
        services.AddScoped<IPaymentsService, PaymentsService>();
        services.AddScoped<IProviderAPIService, ProviderAPIService>();
        services.AddScoped<IMemberSiteManagementService, MemberSiteManagementService>();
        services.AddScoped<IPendingDepositService, PendingDepositService>();
        services.AddScoped<IPendingWithdrawalService, PendingWithdrawalService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IPromotionService, PromotionService>();
        services.AddScoped<IBankService, BankService>();

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
}