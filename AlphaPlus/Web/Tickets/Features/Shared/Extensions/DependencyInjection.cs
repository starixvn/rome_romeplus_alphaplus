using System.Net;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.Handlers.Parlay;
using AlphaPlus.Tickets.Site.Features.BetLists.Models;
using AlphaPlus.Tickets.Site.Features.Forecast;
using AlphaPlus.Tickets.Site.Features.Results.Normal;
using AlphaPlus.Tickets.Site.Features.Results.ThirdParty;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Tickets.Site.Features.TotalBets;
using Fanex.BetDetails.BetBuilder.Services;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.RemotingRef;
using Fanex.BetList.Core.TicketHelper;
using Fanex.Result.Core.Base;
using Fanex.Result.Core.ConfigurationHelper;
using Fanex.Result.Core.KenoLotteryTG;
using Fanex.Result.Core.LinkedGames;
using Fanex.Result.Core.RemotingRef;
using Fanex.Result.Core.SabaClub;
using Fanex.Result.Core.SportsLottery;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<AppSettings>(configuration)
            .AddSingleton<IUser, User>()
            .AddSingleton<Fanex.Caching.ICacheService, Fanex.Caching.CacheService>()
            .AddSingleton<ICacheRefService, CacheRefService>()
            .AddSingleton<IEncryption, Encryption>()
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
        .AddScoped(typeof(Workbook<>), typeof(NpoiWorkbook<>))
        .AddSingleton<IBetListFactory<BetListHTMLBuilder>, BetListHTMLFactory>()
        .AddSingleton<IBetListFactory<BetListExcelBuilder>, BetListExcelFactory>()
        .AddSingleton<IBetListHTMLBuilder>(sp => sp.GetService<IBetListFactory<BetListHTMLBuilder>>().CreateBuilder())
        .AddSingleton<IBetListExcelBuilder>(sp => sp.GetService<IBetListFactory<BetListExcelBuilder>>().CreateBuilder())
        //.AddSingleton<IBetBuilderService, BetBuilderService>()
        .AddSingleton<IComboParlayDetailWorkbook, ComboParlayDetailWorkbook>();

    public static IServiceCollection AddResultsSiteServices(this IServiceCollection services, IConfiguration configuration)
    {
        ResultCoreManager.RegisterRefDataHelper(new RefDataHelper());

        return services
            .AddScoped<IConfigurationHelper, ConfigurationHelper>()
            .AddSingleton<IResultServiceFactory, ResultServiceFactory>()
            .AddSingleton<IResultEngineAdapter, ResultEngineAdapter>()
            .AddScoped<IKenoResult>(_ => new KenoResult(configuration.GetSection("KenoResultConfigs").Get<KenoLotteryTGConfigs>()))
            .AddScoped<ILotteryResult>(_ => new LotteryResult(configuration.GetSection("LotteryResultConfigs").Get<KenoLotteryTGConfigs>()))
            .AddScoped<ITableGamesResult>(_ => new TableGamesResult(configuration.GetSection("TableGameResultConfigs").Get<KenoLotteryTGConfigs>()))
            .AddScoped<ISportsLotteryResult>(_ => new SportsLotteryResult(configuration.GetSection("SportsLotteryResultConfigs").Get<SportsLotteryConfigs>()))
            .AddScoped<IVirtualGamesResult>(_ => new VirtualGamesResult(configuration.GetSection("VirtualGamesResultConfigs").Get<KenoLotteryTGConfigs>()))
            .AddScoped<ILinkedGamesResult>(_ => new LinkedGamesResult(configuration.GetSection("Soccer5ResultConfigs").Get<LinkedGamesConfigs>()))
            .AddScoped<ISabaClubResult>(_ => new SabaClubResult(configuration.GetSection("SabaClubConfigs").Get<SabaClubConfigs>()));
    }
}
