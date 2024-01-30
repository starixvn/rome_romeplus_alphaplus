using System.Net;
using AlphaPlus.Core.Models;
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
        const string AppName = "AlphaPlus";
        var redisConfiguration = configuration.GetValue<string>("RedisConfiguration");
        services
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfiguration;
                options.InstanceName = AppName;
            })
            .AddSession()
            .AddDataProtection()
            .SetApplicationName(AppName)
            .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisConfiguration));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton<IUserSession, UserSessionExt>()
            .AddSingleton<Fanex.Caching.ICacheService, Fanex.Caching.CacheService>()
            .Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // https://www.milanjovanovic.tech/blog/the-right-way-to-use-httpclient-in-dotnet
        // Why You Should Avoid Typed Clients In Singleton Services
        const int HttpClientPooledConnectionLifetime = 15;
        var ticketsAPIBaseUrl = configuration.GetValue<string>("TicketsAPIBaseUrl");
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
            .AddHttpClient<IScoreMapService, ScoreMapService>(httpClient
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
            .AddHttpClient<ISoccerService, SoccerService>(httpClient
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
