using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.PartialUM.Extensions;
using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Features.Statement;
using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;
using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;
using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.BonusDetail;
using AlphaPlus.Reports.Features.IPMatching;
using AlphaPlus.Reports.Features.BetChoiceMatching;
using AlphaPlus.Utilities.IP;
using AlphaPlus.Reports.Features.WinLossAnalysisChart;
using AlphaPlus.Reports.Features.WinLossAnalysis;
using AlphaPlus.Reports.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Features.SuperWinLossDetailByPT;
using AlphaPlus.Reports.Features.WinLossByProduct;
using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.Customer;
using Fanex.Data.Repository;
using Fanex.Caching;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddReportsServices(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<int, bool> canViewProduct)
    {
        services
            .AddAllowedSitesToShareLoadConfig(configuration)
            .Configure<AppSettings>(configuration)
            .AddSingleton<IPService, IPServiceImpl>()
            .AddSingleton<IAutoFollowBetService, AutoFollowBetService>()
            .AddSingleton<ICacheRefService, CacheRefService>()
            .AddSingleton<IPartialUMDataAccess, PartialUMDataAccess>()
            .AddPartialUM(configuration)
            .AddSingleton<IProductService>(sp => new ProductService(
                sp.GetRequiredService<IDynamicRepository>(),
                sp.GetRequiredService<ICacheService>(),
                canViewProduct,
                sp.GetRequiredService<ICacheRefService>(),
                sp.GetRequiredService<IOptions<AppSettings>>()))
            .AddSingleton<IReportDateService, ReportDateService>()
            .AddSingleton<ILeagueService, LeagueService>()
            .AddSingleton<ICustomerService, CustomerService>()
            .AddSingleton<IOutstandingService, OutstandingService>()
            .AddSingleton<IEnteredResultService, EnteredResultService>()
            .AddSingleton<IWinLossService, WinLossService>()
            .AddSingleton<IWinLossDetailService, WinLossDetailService>()
            .AddSingleton<IMatchWinLossDetailService, MatchWinLossDetailService>()
            .AddSingleton<IWinLossAnalysisService, WinLossAnalysisService>()
            .AddSingleton<IWinLossAnalysisChartService, WinLossAnalysisChartService>()
            .AddSingleton<IWinLossByProductService, WinLossByProductService>()
            .AddSingleton<ISuperWinLossDetailByPTService, SuperWinLossDetailByPTService>()
            .AddSingleton<ICommissionByBetTypeService, CommissionByBetTypeService>()
            .AddSingleton<IConsolidationService, ConsolidationService>()
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

    private static IServiceCollection AddAllowedSitesToShareLoadConfig(this IServiceCollection services, IConfiguration configuration)
    {
        IDictionary<string, string> siteNameToIP = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var allowedSitesToShareLoadConfig = configuration.GetSection("AllowedSitesToShareLoad");

        foreach (var item in allowedSitesToShareLoadConfig.GetChildren())
        {
            var siteNames = allowedSitesToShareLoadConfig.GetValue<string>(item.Key).ToList();

            foreach (var site in siteNames)
            {
                siteNameToIP[site.Trim()] = item.Key;
            }
        }

        var allowedSitesToShareLoad = new AllowedSitesToShareLoad(siteNameToIP);

        return services.AddSingleton(allowedSitesToShareLoad);
    }
}