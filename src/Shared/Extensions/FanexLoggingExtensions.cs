using Fanex.Logging;
using Fanex.Logging.RabbitMQ;
using Fanex.Logging.Sentry;

namespace Shared.Extensions
{
    internal static class FanexLoggingExtensions
    {
        internal static IServiceCollection AddFanexLogging(this IServiceCollection services, IConfiguration configuration)
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

            var sentryUrl = configuration["Fanex.Logging:SentryBackendUrl"];
            if (!string.IsNullOrWhiteSpace(sentryUrl))
            {
                yield return new SentryLogging(new SentryEngineOptions
                {
                    Dsn = sentryUrl,
                    FrontEndDsn = configuration["Fanex.Logging:SentryUrl"]
                });
            }
        }
    }
}