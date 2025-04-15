using AlphaPlus.BetsForecast.Infrastructure.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets;
using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaPlus.BetsForecast.Infrastructure.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddSingleton<ICacheRefService, CacheRefService>()
        .AddSingleton<IPartialUMDataAccess, PartialUMDataAccess>()
        .AddPartialUM(configuration)
        .AddSingleton<ITotalBetsService, TotalBetsService>()
        .AddSingleton<IForecastService, ForecastService>()
        .AddSingleton<IScoreMapService, ScoreMapService>()
        .AddSingleton<ISoccerService, SoccerService>();
}