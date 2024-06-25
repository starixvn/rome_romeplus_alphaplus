using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Extensions;
using AlphaPlus.Results.Features.Normal;
using AlphaPlus.Results.Features.Outright;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaPlus.Results.Features.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddResultsServices(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddSingleton<IPartialUMDataAccess, PartialUMDataAccess>()
        .AddPartialUM(configuration)
        .AddSingleton<IResultsService, ResultsService>()
        .AddSingleton<IParlayService, ParlayService>()
        .AddSingleton<ISportsbookService, SportsbookService>()
        .AddSingleton<INumberGameService, NumberGameService>()
        .AddSingleton<IVirtualRacingService, VirtualRacingService>()
        .AddSingleton<IBitcoinService, BitcoinService>()
        .AddSingleton<IOutrightService, OutrightService>();
}