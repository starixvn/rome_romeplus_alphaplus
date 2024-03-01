using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.PartialUM.Extensions;
using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Legends;
using AlphaPlus.Reports.Features.ProgressiveContribution;

namespace AlphaPlus.Reports.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddReportsServices(this IServiceCollection services, IConfiguration configuration)
        => services
        .Configure<AppSettings>(configuration)
        .AddSingleton<IAutoFollowBetService, AutoFollowBetService>()
        .AddSingleton<ICacheRefService, CacheRefService>()
        .AddSingleton<IPartialUMDataAccess, PartialUMDataAccess>()
        .AddPartialUM(configuration)
        .AddSingleton<IProductService, ProductService>()
        .AddSingleton<IReportDateService, ReportDateService>()
        .AddSingleton<ILengendService, LengendService>()
        .AddSingleton<IOutstandingService, OutstandingService>()
        .AddSingleton<IEnteredResultService, EnteredResultService>()
        .AddSingleton<IProgressiveContributionService, ProgressiveContributionService>();
}