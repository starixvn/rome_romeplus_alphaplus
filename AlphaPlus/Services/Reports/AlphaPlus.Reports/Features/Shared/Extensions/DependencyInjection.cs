using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.PartialUM.Extensions;
using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Legends;
using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Features.Statement;
using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;
using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;
using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.BonusDetail;
using AlphaPlus.Reports.Features.IPMatching;
using AlphaPlus.Reports.Features.BetChoiceMatching;
using AlphaPlus.Utilities.IP;

namespace AlphaPlus.Reports.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddReportsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<AppSettings>(configuration)
            .AddSingleton<IPService, IPServiceImpl>()
            .AddSingleton<IAutoFollowBetService, AutoFollowBetService>()
            .AddSingleton<ICacheRefService, CacheRefService>()
            .AddSingleton<IPartialUMDataAccess, PartialUMDataAccess>()
            .AddPartialUM(configuration)
            .AddSingleton<IProductService, ProductService>()
            .AddSingleton<IReportDateService, ReportDateService>()
            .AddSingleton<ILeagueService, LeagueService>()
            .AddSingleton<ILengendService, LengendService>()
            .AddSingleton<IOutstandingService, OutstandingService>()
            .AddSingleton<IEnteredResultService, EnteredResultService>()
            .AddSingleton<ICommissionByBetTypeService, CommissionByBetTypeService>()
            .AddSingleton<IStatementService, StatementService>()
            .AddSingleton<IProgressiveContributionService, ProgressiveContributionService>()
            .AddSingleton<IBonusDetailService, BonusDetailService>()
            .AddSingleton<IBetCreditHistoryService, BetCreditHistoryService>()
            .AddSingleton<IMoveStatementHistoryService, MoveStatementHistoryService>()
            .AddSingleton<IIPMatchingService, IPMatchingService>()
            .AddSingleton<IBetChoiceMatchingService, BetChoiceMatchingService>();

        var serviceProvider = services.BuildServiceProvider();
        ReportDateService.Instance = serviceProvider.GetRequiredService<IReportDateService>();

        return services;
    }
}