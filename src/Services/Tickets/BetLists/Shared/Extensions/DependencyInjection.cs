using AlphaPlus.IPInfo.DataAccess;
using AlphaPlus.IPInfo.Models;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.DataAccesses;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Utilities.IP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.BetLists.Shared.DataAccesses;
using AlphaPlus.BetLists.RunningBetListByBetTypeAndMatch;
using AlphaPlus.BetLists.SearchTickets;
using AlphaPlus.BetLists.CorrectScore;
using AlphaPlus.BetLists.RunningBetListByBetType;
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
using AlphaPlus.BetLists.RunningBetList;
using AlphaPlus.BetLists.MemberBetList;
using System.Diagnostics.CodeAnalysis;

namespace AlphaPlus.BetLists.Shared.Extensions;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddBetListsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<Func<int, bool>>(_ => UserPermission.IsShowOrNotExistProduct)
            .AddSingleton<IIPInfoDataAccess, IPInfoDataAccess>()
            .AddSingleton<ISmartPunterDataAccess, SmartPunterDataAccess>()
            .AddSingleton<IPService, IPServiceImpl>()
            .AddSingleton(_ => configuration.GetSection("IPInformationConfig").Get<IPInformationConfig>())
            .AddSingleton<IIPInfoService, IPInfoService>()
            .AddSingleton<ISmartPunterService, SmartPunterService>()
            .AddSingleton<ISmartPunterBetListMonitor, SmartPunterBetListMonitor>()
            .AddSingleton<IAutoFollowBetService, AutoFollowBetService>()
            .AddSingleton<IForecastService, ForecastService>()
            .AddSingleton<IReportDateService, ReportDateService>()
            .AddSingleton<IProductService, ProductService>()
            .AddSingleton<ILastBetsService, LastBetsService>()
            .AddSingleton<IFilterBetsService, FilterBetsService>()
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
            .AddSingleton<ICasinoDetailsService, CasinoDetailsService>()
            .AddSingleton<IRunningBetListService, RunningBetListService>()
            .AddSingleton<IMemberBetListService, MemberBetListService>();

        var serviceProvider = services.BuildServiceProvider();
        ReportDateService.Instance = serviceProvider.GetRequiredService<IReportDateService>();
        CacheRefServiceHelper.Instance = serviceProvider.GetRequiredService<IRemotingRefService>();

        return services;
    }
}