using Fanex.Alpha.Before.Configurations;
using Fanex.Alpha.Before.NetThrottling;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.AspNetCore;
using Fanex.Logging.RabbitMQ;
using Fanex.Logging.Sentry;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ONEbook.UM.DependencyInjection;

namespace Fanex.Alpha.Before.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddONEbookUM(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOneBookUmForWeb(configuration);
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddFanexLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var logEngines = new List<ILogEngine>
        {
            new RabbitMQLogging(configuration["Fanex.Logging:RabbitMQConnectionStrings"])
        };

        var sentryUrl = configuration["Fanex.Logging:SentryUrl"];
        if (!string.IsNullOrWhiteSpace(sentryUrl))
        {
            logEngines.Add(new SentryLogging(new SentryEngineOptions
            {
                Dsn = sentryUrl
            }));
        }

        LogManager
                .SetDefaultLogCategory(configuration["Fanex.Logging:DefaultCategory"])
                .Use(logEngines);

        services.AddSingleton(Logger.Log);

        ExceptionHandler
            .UseConfig(new LoggingConfiguration()
            .GetHttpHeaderInfo());

        return services;
    }

    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CommonOptions>(configuration.GetSection(CommonOptions.Common));
        services.Configure<AutomationTestOptions>(configuration.GetSection(AutomationTestOptions.AutomationTest));
        services.Configure<LoginThemeOptions>(configuration.GetSection(LoginThemeOptions.LoginTheme));
        services.Configure<ManualUMOptions>(configuration.GetSection(ManualUMOptions.ManualUM));

        return services;
    }

    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());

        return services;
    }

    public static IServiceCollection AddNetThrottling(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ThrottlePolicy>(configuration.GetSection("ThrottlePolicy"));
        services.AddSingleton<INetThrottler, NetThrottler>();

        return services;
    }
}