using System.Diagnostics.CodeAnalysis;
using System.Net;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.Handlers.Customer;
using AlphaPlus.Tickets.Site.Features.BetLists.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAllowedSitesToShareLoadConfig(configuration)
            .Configure<AppSettings>(configuration)
            .AddSingleton<IUser, User>()
            .AddSingleton<ICacheRefService, CacheRefService>()
            .AddSingleton<IEncryption, Encryption>();
        services.AddHttpClient<ICustomerClient, CustomerClient>(configuration.GetValue<string>("Fanex.Customer:ServiceUri"));

        return services;
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

    private static IServiceCollection AddHttpClient<TClient, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        this IServiceCollection services,
        string baseUrl,
        int pooledConnectionLifetime = 15)
            where TClient : class
            where TImplementation : class, TClient
    {
        services
            .AddHttpClient<TClient, TImplementation>(httpClient => httpClient.BaseAddress = new Uri(baseUrl))
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            })
            .ConfigurePrimaryHttpMessageHandler(_ => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(pooledConnectionLifetime)
            })
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        return services;
    }
}
