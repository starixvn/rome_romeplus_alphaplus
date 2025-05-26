using AgencyCash.API.Payments.Services;

namespace AgencyCash.API.Payments;

public static class PaymentsModule
{
    public static IServiceCollection RegisterPaymentsModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Register HTTP clients for payment providers
        services.AddHttpClient("U2Pay", client =>
        {
            client.BaseAddress = new Uri(configuration["PaymentProviders:U2Pay:BaseUrl"]);
            client.DefaultRequestHeaders.Add("X-API-Key", configuration["PaymentProviders:U2Pay:ApiKey"]);
        });

        services.AddHttpClient("DirePay", client =>
        {
            client.BaseAddress = new Uri(configuration["PaymentProviders:DirePay:BaseUrl"]);
            client.DefaultRequestHeaders.Add("X-API-Key", configuration["PaymentProviders:DirePay:ApiKey"]);
        });

        // Register services
        services.AddSingleton<IDepositService, DepositService>();

        return services;
    }
}