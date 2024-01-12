using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.RabbitMQ;
using Fanex.Logging.Sentry;
using Fanex.RemotingRef;
using Sentry;


[ExcludeFromCodeCoverage]
internal static class CommonExtensions
{

    public static IServiceCollection AddFanexLibs(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddFanexLogging(configuration)
        .AddFanexDataWithSqlServer(configuration)
        .AddRemotingRef(configuration);

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

    private static IServiceCollection AddFanexDataWithSqlServer(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddFanexSql(configuration)
        .AddSingleton<IDynamicRepository, DynamicRepository>();

    private static IServiceCollection AddRemotingRef(this IServiceCollection services, IConfiguration configuration)
    {
        RefDataTypeContainer
            .ConnectTo(new Uri(configuration["RemotingRefUrl"]))
            .FromApplication(Assembly.GetExecutingAssembly().FullName);

        return services.AddSingleton<IRemotingRefService, RemotingRefService>();
    }
}
