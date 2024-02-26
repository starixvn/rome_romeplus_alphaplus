using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.PartialUM.Extensions;
using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.Reports.Features.Outstanding;

namespace AlphaPlus.Reports.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddReportsServices(this IServiceCollection services, IConfiguration configuration)
        => services
        .Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)))
        .AddSingleton<IAutoFollowBetService, AutoFollowBetService>()
        .AddSingleton<ICacheRefService, CacheRefService>()
        .AddSingleton<IPartialUMDataAccess, PartialUMDataAccess>()
        .AddPartialUM(configuration)
        .AddSingleton<IProductService, ProductService>()
        .AddSingleton<IReportDateService, ReportDateService>()
        .AddSingleton<IOutstandingService, OutstandingService>();
}