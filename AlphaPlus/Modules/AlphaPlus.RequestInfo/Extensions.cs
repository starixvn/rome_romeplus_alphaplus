using Fanex.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AlphaPlus.RequestInfo;

public static class Extensions
{
    public static IServiceCollection AddRequestInfo(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<IPService, IPServiceImpl>();
        services.AddSingleton<IPHelper, IPHelperImpl>();
        services.AddSingleton<IDeviceHelper, DeviceHelper>();

        return services;
    }
}