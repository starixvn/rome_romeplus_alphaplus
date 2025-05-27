using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using PaymentService.API.Payments.Services;

namespace PaymentService.API.Shared;

public static class PaymentServices
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFanexSql(configuration)
            .AddSingleton<IDynamicRepository, DynamicRepository>()
            .AddSingleton<ISiteDBService, SiteDBService>()
            .AddSingleton<IWithdrawalService, WithdrawalService>()
            .AddSingleton<IPaymentProviderService, U2PayProviderService>()
            .AddSingleton<IPaymentProviderService, DirePayProviderService>();

        return services;
    }
} 