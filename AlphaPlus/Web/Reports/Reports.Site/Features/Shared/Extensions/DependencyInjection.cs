using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.BetChoiceMatching;
using AlphaPlus.Reports.Site.Features.BetCreditTracking;
using AlphaPlus.Reports.Site.Features.CommissionByBetType;
using AlphaPlus.Reports.Site.Features.Consolidation;
using AlphaPlus.Reports.Site.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.IPMatching;
using AlphaPlus.Reports.Site.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Site.Features.ProgressiveContribution;
using AlphaPlus.Reports.Site.Features.Statement;
using AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;
using AlphaPlus.Reports.Site.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.WinLossAnalysis;
using AlphaPlus.Reports.Site.Features.WinLossByProduct;
using AlphaPlus.Reports.Site.Features.WinLossDetail;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.RemotingRef;
using Fanex.BetList.Core.TicketHelper;

namespace AlphaPlus.Reports.Site.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddSingleton<Func<int, bool>>(_ => UserPermission.IsShowOrNotExistProduct)
        .AddReportsServices(configuration)
        .AddTransient<IEnteredResultWorkbook, EnteredResultWorkbook>()
        .AddTransient<IWinLossWorkbook, WinLossWorkbook>()
        .AddTransient<IWinLossDetailWorkbook, WinLossDetailWorkbook>()
        .AddTransient<IMatchWinLossDetailWorkbook, MatchWinLossDetailWorkbook>()
        .AddTransient<IWinLossAnalysisWorkbook, WinLossAnalysisWorkbook>()
        .AddTransient<IWinLossByProductWorkbook, WinLossByProductWorkbook>()
        .AddTransient<ISuperWinLossDetailByWorkbook, SuperWinLossDetailByWorkbook>()
        .AddTransient<ICommissionByBetTypeWorkbook, CommissionByBetTypeWorkbook>()
        .AddTransient<IConsolidationByUsernameWorkbook, ConsolidationByUsernameWorkbook>()
        .AddTransient<IConsolidationWorkbook, ConsolidationWorkbook>()
        .AddTransient<ISMAStatementWorkbook, SMAStatementWorkbook>()
        .AddTransient<IStatementDetailWorkbook, StatementDetailWorkbook>()
        .AddTransient<IMemberStatementWorkbook, MemberStatementWorkbook>()
        .AddTransient<IProgressiveContributionWookbook, ProgressiveContributionWookbook>()
        .AddTransient<IBetCreditHistoryWorkbook, BetCreditHistoryWorkbook>()
        .AddTransient<IMoveStatementHistoryWorkbook, MoveStatementHistoryWorkbook>()
        .AddTransient<IIPMatchingWorkbook, IPMatchingWorkbook>()
        .AddTransient<IBetChoiceMatchingWorkbook, BetChoiceMatchingWorkbook>()
        // for BetList export excel
        .AddSingleton<ICacheRef, CacheRef>()
        .AddScoped<ITicketHelper, TicketHelper>()
        .AddScoped<IBetListFactory<BetListExcelBuilder>, BetListExcelFactory>()
        .AddScoped<IBetListExcelBuilder>(sp => sp.GetRequiredService<IBetListFactory<BetListExcelBuilder>>().CreateBuilder());
}
