using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Site.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddSingleton<Func<int, bool>>(_ => UserPermission.HasPermissionOnProduct)
        .AddReportsServices(configuration);
}
