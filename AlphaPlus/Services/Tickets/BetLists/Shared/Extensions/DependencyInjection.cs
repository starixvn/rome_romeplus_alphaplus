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
using AlphaPlus.BetLists.DailyStatement;
using AlphaPlus.BetLists.CancelledBets;
using AlphaPlus.BetLists.MoveStatementHistory;
using AlphaPlus.BetLists.CashOutHistory;
using AlphaPlus.BetLists.CasinoDetails;
using AlphaPlus.BetLists.LastBets;
using AlphaPlus.BetLists.RiskAnalysisIPMatching;
using AlphaPlus.BetLists.WinLossByMatch;
using AlphaPlus.BetLists.ParlayWinLossByMatch;
using AlphaPlus.BetLists.WinLoss;
using AlphaPlus.BetLists.JackpotDetails;
using AlphaPlus.BetLists.BetBuilder;
using AlphaPlus.BetLists.Shared.Helpers;
using Fanex.RemotingRef;

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
            //.AddSingleton<IProductService, ProductService>()
            .AddSingleton<ILastBetsService, LastBetsService>()
            .AddSingleton<IMonitoringBetsService, MonitoringBetsService>()
            .AddSingleton<ISearchTicketsService, SearchTicketsService>()
            .AddSingleton<ICancelledBetService, CancelledBetService>()
            .AddSingleton<ICorrectScoreService, CorrectScoreService>()
            .AddSingleton<IParlayService, ParlayService>()
            .AddSingleton<IRunningBetListByBetTypeService, RunningBetListByBetTypeService>()
            .AddSingleton<IRunningBetListByBetTypeAndMatchService, RunningBetListByBetTypeAndMatchService>()
            .AddSingleton<IWinLossService, WinLossService>()
            .AddSingleton<IWinLossByMatchService, WinLossByMatchService>()
            .AddSingleton<IParlayWinLossByMatchService, ParlayWinLossByMatchService>()
            .AddSingleton<IDailyStatementService, DailyStatementService>()
            .AddSingleton<IMoveStatementHistoryService, MoveStatementHistoryService>()
            .AddSingleton<IRiskAnalysisIPMatchingService, RiskAnalysisIPMatchingService>()
            .AddSingleton<IBetBuilderService, BetBuilderService>()
            .AddSingleton<IJackpotDetailsService, JackpotDetailsService>()
            .AddSingleton<ICashOutHistoryService, CashOutHistoryService>()
            .AddSingleton<ICasinoDetailsService, CasinoDetailsService>();

        var serviceProvider = services.BuildServiceProvider();
        ReportDateService.Instance = serviceProvider.GetRequiredService<IReportDateService>();
        CacheRefServiceHelper.Instance = serviceProvider.GetRequiredService<IRemotingRefService>();

        return services;
    }
}