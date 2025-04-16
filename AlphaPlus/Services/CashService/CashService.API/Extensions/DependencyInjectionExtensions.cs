using AlphaPlus.CashService.API.Services;

namespace AlphaPlus.CashService.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCashServiceDependencies(this IServiceCollection services)
    {
        services.AddScoped<IManagePaymentsService, ManagePaymentsService>();
        return services;
    }
} 