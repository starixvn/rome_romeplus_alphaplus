using AlphaPlus.BetsForecast.Infrastructure.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Services;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets;

namespace AlphaPlus.Tickets.API.Extensions;

public static class InfrastructureExtenstions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
        .AddSingleton<ICacheRefService, CacheRefService>()
        .AddSingleton<ITotalBetsService, TotalBetsService>()
        .AddSingleton<ILeagueService, LeagueService>()
        .AddSingleton<IForecastService, ForecastService>()
        .AddSingleton<IScoreMapService, ScoreMapService>();
}