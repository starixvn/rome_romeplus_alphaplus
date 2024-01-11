using System.Net;
using AlphaPlus.Tickets.Site.Features.Forecast;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Tickets.Site.Features.TotalBets;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class InfrastructureExtenstions
{
    public static IServiceCollection AddSharedSession(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetValue<string>("RedisConfiguration");
        services
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfiguration;
                options.InstanceName = "AlphaPlus";
            })
            .AddSession(options =>
            {
                options.Cookie.Name = ".AlphaPlus.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            })
            .AddDataProtection()
            .SetApplicationName("AlphaPlus")
            .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisConfiguration));

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

        // https://www.milanjovanovic.tech/blog/the-right-way-to-use-httpclient-in-dotnet
        // Why You Should Avoid Typed Clients In Singleton Services
        const int HttpClientPooledConnectionLifetime = 15;
        var ticketsAPIBaseUrl = configuration.GetValue<string>("TicketsAPIBaseUrl");
        services
            .AddHttpClient<IForecastService, ForecastService>(httpClient
                => httpClient.BaseAddress = new Uri(ticketsAPIBaseUrl))
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            })
            .ConfigurePrimaryHttpMessageHandler(_ => new SocketsHttpHandler()
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(HttpClientPooledConnectionLifetime)
            })
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
        services
            .AddHttpClient<ITotalBetsService, TotalBetsService>(httpClient
                => httpClient.BaseAddress = new Uri(ticketsAPIBaseUrl))
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            })
            .ConfigurePrimaryHttpMessageHandler(_ => new SocketsHttpHandler()
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(HttpClientPooledConnectionLifetime)
            })
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        return services;
    }
}
