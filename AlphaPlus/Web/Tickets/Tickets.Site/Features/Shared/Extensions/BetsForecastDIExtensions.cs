using System.Diagnostics.CodeAnalysis;
using System.Net;
using AlphaPlus.Tickets.Site.Features.Forecast;
using AlphaPlus.Tickets.Site.Features.TotalBets;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class BetsForecastDIExtensions
{
    public static IServiceCollection AddBetsForecastServices(this IServiceCollection services, IConfiguration configuration)
    {
        // https://www.milanjovanovic.tech/blog/the-right-way-to-use-httpclient-in-dotnet
        // Why You Should Avoid Typed Clients In Singleton Services
        var ticketsAPIBaseUrl = configuration.GetValue<string>("TicketsAPIBaseUrl");

        return services
            .AddHttpClient<ITotalBetsService, TotalBetsService>(ticketsAPIBaseUrl)
            .AddHttpClient<IForecastService, ForecastService>(ticketsAPIBaseUrl)
            .AddHttpClient<IScoreMapService, ScoreMapService>(ticketsAPIBaseUrl)
            .AddHttpClient<ISoccerService, SoccerService>(ticketsAPIBaseUrl);
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