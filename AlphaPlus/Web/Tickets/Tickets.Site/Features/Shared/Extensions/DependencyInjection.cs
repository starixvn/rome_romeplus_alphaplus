using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddAllowedSitesToShareLoadConfig(configuration)
            .Configure<AppSettings>(configuration)
            .AddSingleton<IUser, User>()
            .AddSingleton<Fanex.Caching.ICacheService, Fanex.Caching.CacheService>()
            .AddSingleton<ICacheRefService, CacheRefService>()
            .AddSingleton<IEncryption, Encryption>();
    }

    private static IServiceCollection AddAllowedSitesToShareLoadConfig(this IServiceCollection services, IConfiguration configuration)
    {
        IDictionary<string, string> siteNameToIP = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var allowedSitesToShareLoadConfig = configuration.GetSection("AllowedSitesToShareLoad");

        foreach (var item in allowedSitesToShareLoadConfig.GetChildren())
        {
            var siteNames = allowedSitesToShareLoadConfig.GetValue<string>(item.Key).ToList();

            foreach (var site in siteNames)
            {
                siteNameToIP[site.Trim()] = item.Key;
            }
        }

        var allowedSitesToShareLoad = new AllowedSitesToShareLoad(siteNameToIP);

        return services.AddSingleton(allowedSitesToShareLoad);
    }
}
