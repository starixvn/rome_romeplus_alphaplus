using AlphaPlus.Tickets.Site.Features.Results.Normal;
using AlphaPlus.Tickets.Site.Features.Results.ThirdParty;
using Fanex.Result.Core.Base;
using Fanex.Result.Core.ConfigurationHelper;
using Fanex.Result.Core.GTCasino;
using Fanex.Result.Core.KenoLotteryTG;
using Fanex.Result.Core.LinkedGames;
using Fanex.Result.Core.RemotingRef;
using Fanex.Result.Core.SabaClub;
using Fanex.Result.Core.SAGaming;
using Fanex.Result.Core.SportsLottery;
using Fanex.Result.Core.WM;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class ResultsDIExtensions
{
    public static IServiceCollection AddResultsSiteServices(this IServiceCollection services, IConfiguration configuration)
    {
        ResultCoreManager.RegisterRefDataHelper(new RefDataHelper());

        return services
            .AddScoped<IConfigurationHelper, ConfigurationHelper>()
            .AddSingleton<IResultServiceFactory, ResultServiceFactory>()
            .AddSingleton<IResultEngineAdapter, ResultEngineAdapter>()
            .AddScoped<IWMResultBuilder>(_ => new WMResultBuilder(configuration.GetValue<string>("WMResultImagePath")))
            .AddScoped<IGTCasinoResultBuilder>(_ => new GTCasinoResultBuilder(configuration.GetValue<string>("GTCasinoResultImagePath")))
            .AddScoped<ISAGamingResultBuilder>(_ => new SAGamingResultBuilder(configuration.GetValue<string>("SAGamingResultImagePath")))
            .AddScoped<IKenoResult>(_ => new KenoResult(configuration.GetSection("KenoResultConfigs").Get<KenoLotteryTGConfigs>()))
            .AddScoped<ILotteryResult>(_ => new LotteryResult(configuration.GetSection("LotteryResultConfigs").Get<KenoLotteryTGConfigs>()))
            .AddScoped<ITableGamesResult>(_ => new TableGamesResult(configuration.GetSection("TableGameResultConfigs").Get<KenoLotteryTGConfigs>()))
            .AddScoped<ISportsLotteryResult>(_ => new SportsLotteryResult(configuration.GetSection("SportsLotteryResultConfigs").Get<SportsLotteryConfigs>()))
            .AddScoped<IVirtualGamesResult>(_ => new VirtualGamesResult(configuration.GetSection("VirtualGamesResultConfigs").Get<KenoLotteryTGConfigs>()))
            .AddScoped<ILinkedGamesResult>(_ => new LinkedGamesResult(configuration.GetSection("Soccer5ResultConfigs").Get<LinkedGamesConfigs>()))
            .AddScoped<ISabaClubResult>(_ => new SabaClubResult(configuration.GetSection("SabaClubConfigs").Get<SabaClubConfigs>()));
    }
}
