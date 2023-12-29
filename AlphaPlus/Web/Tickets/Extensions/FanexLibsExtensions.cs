using Fanex.Logging;
using Fanex.Logging.RabbitMQ;
using Fanex.Logging.Sentry;
using Sentry;

namespace AlphaPlus.Tickets.Site.Extensions;

public static class FanexLibsExtensions
{
    public static IServiceCollection AddFanexLogging(this IServiceCollection services, IConfiguration configuration)
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
}
