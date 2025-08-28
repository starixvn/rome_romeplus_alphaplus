using Fanex.Customer.Client.Configurations;
using Fanex.Customer.Client.Services;

namespace AlphaPlus.CashService.Site.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new CustomerClientConfiguration(
            configuration.GetValue<string>("Fanex.Customer:ServiceUri"),
            configuration.GetValue("Fanex.Customer:SystemId", 2),
            configuration.GetValue("Fanex.Customer:AccessDeniedUrl", "~/User/AccessDenied")));

        services.AddSingleton<ICustomerService, CustomerService>();
        return services;
    }
}