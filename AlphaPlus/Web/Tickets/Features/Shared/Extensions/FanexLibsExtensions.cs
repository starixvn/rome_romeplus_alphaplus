using Fanex.Logging;
using Fanex.Logging.RabbitMQ;
using Fanex.Logging.Sentry;
using Fanex.RemotingRef;
using Sentry;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class FanexLibsExtensions
{
    public static IServiceCollection AddFanexLibs(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddFanexLogging(configuration)
        .AddRemotingRef(configuration)
        .AddFanexCachingMemory();

    private static IServiceCollection AddFanexLogging(this IServiceCollection services, IConfiguration configuration)
    {
        LogManager
            .SetDefaultLogCategory(configuration["Fanex.Logging:DefaultCategory"])
            .Use(GetLogEngines(configuration));

        return services.AddSingleton(Logger.Log);
    }

    private static IEnumerable<ILogEngine> GetLogEngines(IConfiguration configuration)
    {
        var rabbitMQConnectionStrings = configuration["Fanex.Logging:RabbitMQConnectionStrings"];
        if (!string.IsNullOrWhiteSpace(rabbitMQConnectionStrings))
        {
            yield return new RabbitMQLogging(rabbitMQConnectionStrings);
        }

        var sentryUrl = configuration["Fanex.Logging:SentryDsn"];
        if (!string.IsNullOrWhiteSpace(sentryUrl))
        {
            yield return new SentryLogging(new SentryEngineOptions
            {
                Dsn = new Dsn(sentryUrl)
            });
        }
    }

    private static IServiceCollection AddRemotingRef(this IServiceCollection services, IConfiguration configuration)
    {
        RefDataTypeContainer
            .ConnectTo(new Uri(configuration["RemotingRefUrl"]))
            .FromApplication("AlphaPlus.Tickets.Site");

        return services.AddSingleton<IRemotingRefService, RemotingRefService>();
    }

    private static IServiceCollection AddFanexCachingMemory(this IServiceCollection services)
        => services.AddSingleton<Fanex.Caching.ICacheService, Fanex.Caching.CacheService>();
}
