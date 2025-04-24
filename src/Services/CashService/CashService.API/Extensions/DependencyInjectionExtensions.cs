using AlphaPlus.CashService.API.Services;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;

namespace AlphaPlus.CashService.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFanexSql(configuration)
            .AddSingleton<IDynamicRepository, DynamicRepository>();

        services.AddScoped<IPaymentsService, PaymentsService>();
        services.AddScoped<IMemberSiteManagementService, MemberSiteManagementService>();
        services.AddScoped<IPendingDepositService, PendingDepositService>();
        services.AddScoped<ITransactionService, TransactionService>();

        return services;
    }
}