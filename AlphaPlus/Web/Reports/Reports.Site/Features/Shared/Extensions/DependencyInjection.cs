using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.BetCreditTracking;
using AlphaPlus.Reports.Site.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.IPMatching;
using AlphaPlus.Reports.Site.Features.ProgressiveContribution;
using AlphaPlus.Reports.Site.Features.Statement;

namespace AlphaPlus.Reports.Site.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddSingleton<Func<int, bool>>(_ => UserPermission.IsShowOrNotExistProduct)
        .AddSingleton<Utilities.IP.IPService, Utilities.IP.IPServiceImpl>()
        .AddReportsServices(configuration)
        //.AddTransient<IEnteredResultWorkbook, EnteredResultWorkbook>()
        .AddTransient<ISMAStatementWorkbook, SMAStatementWorkbook>()
        .AddTransient<IStatementDetailWorkbook, StatementDetailWorkbook>()
        .AddTransient<IMemberStatementWorkbook, MemberStatementWorkbook>()
        .AddTransient<IProgressiveContributionWookbook, ProgressiveContributionWookbook>()
        .AddTransient<IBetCreditHistoryWorkbook, BetCreditHistoryWorkbook>()
        .AddTransient<IMoveStatementHistoryWorkbook, MoveStatementHistoryWorkbook>()
        .AddTransient<IIPMatchingWorkbook, IPMatchingWorkbook>();
}
