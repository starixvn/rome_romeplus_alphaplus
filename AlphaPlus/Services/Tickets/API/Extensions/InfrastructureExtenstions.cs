using AlphaPlus.BetsForecast.Infrastructure.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets;

namespace AlphaPlus.Tickets.API.Extensions;

public static class InfrastructureExtenstions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
        .AddSingleton<ILeagueRepository, LeagueRepository>()
        .AddSingleton<IForecastRepository, ForecastRepository>()
        .AddSingleton<ITotalBetsRepository, TotalBetsRepository>();
}