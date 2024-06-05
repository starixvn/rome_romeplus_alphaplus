using AlphaPlus.IPInfo.DataAccess;
using AlphaPlus.IPInfo.Models;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.DataAccesses;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Utilities.IP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.BetLists.Shared.DataAccesses;
using AlphaPlus.BetLists.BetsForecast;
using AlphaPlus.BetLists.SearchTickets;
using AlphaPlus.BetLists.CorrectScore;
using AlphaPlus.BetLists.Running;
using AlphaPlus.BetLists.Parlay;

namespace AlphaPlus.BetLists.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddBetListsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IIPInfoDataAccess, IPInfoDataAccess>()
            .AddSingleton<ISmartPunterDataAccess, SmartPunterDataAccess>()
            .AddSingleton<IPService, IPServiceImpl>()
            .AddSingleton(_ => configuration.GetSection("IPInformationConfig").Get<IPInformationConfig>())
            .AddSingleton<IIPInfoService, IPInfoService>()
            .AddSingleton<ISmartPunterService, SmartPunterService>()
            .AddSingleton(sp => SmartPunterBetListMonitor.CreateInstance(
                sp.GetService<IUserSession>(),
                sp.GetService<ISmartPunterService>(),
                IPHelper.GetClientIP))
            .AddSingleton<IForecastService, ForecastService>()
            .AddSingleton<IReportDateService, ReportDateService>()
            .AddSingleton<ISearchTicketsService, SearchTicketsService>()
            .AddSingleton<ICorrectScoreService, CorrectScoreService>()
            .AddSingleton<IParlayService, ParlayService>()
            .AddSingleton<IRunningBetListByBetTypeService, RunningBetListByBetTypeService>()
            .AddSingleton<IRunningBetListByBetTypeAndMatchService, RunningBetListByBetTypeAndMatchService>();

        var serviceProvider = services.BuildServiceProvider();
        ReportDateService.Instance = serviceProvider.GetRequiredService<IReportDateService>();

        return services;
    }
}