using AlphaPlus.BetsForecast.Infrastructure.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaPlus.BetsForecast.Infrastructure.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
        .AddSingleton<ICacheRefService, CacheRefService>()
        .AddSingleton<ITotalBetsService, TotalBetsService>()
        .AddSingleton<IForecastService, ForecastService>()
        .AddSingleton<IScoreMapService, ScoreMapService>()
        .AddSingleton<ISoccerService, SoccerService>();
}