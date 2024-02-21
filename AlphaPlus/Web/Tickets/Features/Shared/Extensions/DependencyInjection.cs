using System.Net;
using AlphaPlus.Tickets.Site.Features.BetLists.Models;
using AlphaPlus.Tickets.Site.Features.Forecast;
using AlphaPlus.Tickets.Site.Features.TotalBets;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.RemotingRef;
using Fanex.BetList.Core.TicketHelper;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IUser, User>()
            .AddSingleton<Fanex.Caching.ICacheService, Fanex.Caching.CacheService>()
            .AddBetListsCore();

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

    private static IServiceCollection AddBetListsCore(this IServiceCollection services)
        => services
        .AddSingleton<ICacheRef, CacheRef>()
        .AddSingleton<ITicketHelper, TicketHelper>()
        //.AddSingleton(typeof(Workbook<>), typeof(NpoiWorkbook<>), Lifestyle.Scoped)
        .AddSingleton<IBetListFactory<BetListHTMLBuilder>, BetListHTMLFactory>()
        .AddSingleton<IBetListFactory<BetListExcelBuilder>, BetListExcelFactory>()
        .AddSingleton<IBetListHTMLBuilder>(sp => sp.GetService<IBetListFactory<BetListHTMLBuilder>>().CreateBuilder())
        .AddSingleton<IBetListExcelBuilder>(sp => sp.GetService<IBetListFactory<BetListExcelBuilder>>().CreateBuilder());
    //.AddSingleton<ComboParlayDetailWorkbook, ComboParlayDetailWorkbookImpl>();
}
