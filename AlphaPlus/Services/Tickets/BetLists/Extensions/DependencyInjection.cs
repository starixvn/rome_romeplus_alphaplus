using Alpha.IPInfo.DataAccess;
using AlphaPlus.BetLists.DataAccesses;
using AlphaPlus.BetLists.Services;
using AlphaPlus.SmartPunter.DataAccesses;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaPlus.BetLists.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddBetListsServices(this IServiceCollection services)
        => services
        .AddSingleton<IIPInfoDataAccess, IPInfoDataAccess>()
        .AddSingleton<ISmartPunterDataAccess, SmartPunterDataAccess>()
        .AddSingleton<IForecastService, ForecastService>()
        .AddSingleton<IRunningBetListByBetTypeAndMatchService, RunningBetListByBetTypeAndMatchService>();
}