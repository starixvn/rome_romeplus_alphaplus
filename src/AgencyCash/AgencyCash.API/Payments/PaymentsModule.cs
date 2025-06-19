using System.Diagnostics.CodeAnalysis;
using AgencyCash.API.Payments.Services;

namespace AgencyCash.API.Payments;

[ExcludeFromCodeCoverage]
public static class PaymentsModule
{
    public static IServiceCollection RegisterPaymentsModule(this IServiceCollection services, IConfiguration configuration)
    => services
        .AddSingleton<IDepositService, DepositService>()
        .AddSingleton<IWithdrawalService, WithdrawalService>();
}