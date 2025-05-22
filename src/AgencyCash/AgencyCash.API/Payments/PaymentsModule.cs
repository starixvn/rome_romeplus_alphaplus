using AgencyCash.API.Payments.Services;

namespace AgencyCash.API.Payments;

public static class PaymentsModule
{
    public static IServiceCollection RegisterPaymentsModule(this IServiceCollection services)
        => services.AddSingleton<IDepositService, DepositService>();
}