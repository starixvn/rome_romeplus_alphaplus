namespace AlphaPlus.CashService.API.Extensions;

using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.RemotingRef;
using Features.MemberSites.Services;
using Features.Payments.Services;
using Features.PendingDeposit.Services;
using Features.PendingWithdrawal.Services;
using Features.Promotion.Services;
using Features.Shared.Services;
using Features.Transactions.Services;

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