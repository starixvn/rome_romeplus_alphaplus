using System.Net;
using AlphaPlus.Tickets.Site.Features.Forecast;
using AlphaPlus.Tickets.Site.Features.TotalBets;
using Fanex.Data.Repository;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IDynamicRepository, DynamicRepository>()
            .AddSingleton<Fanex.Caching.ICacheService, Fanex.Caching.CacheService>();

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
