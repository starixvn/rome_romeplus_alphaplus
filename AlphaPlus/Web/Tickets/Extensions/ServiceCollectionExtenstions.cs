using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace AlphaPlus.Tickets.Site.Extensions;

public static class ServiceCollectionExtenstions
{
    public static IServiceCollection AddSharedSession(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetValue<string>("RedisConfiguration");
        services
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfiguration;
                options.InstanceName = "AlphaPlus";
            })
            .AddSession(options =>
            {
                options.Cookie.Name = ".AlphaPlus.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            })
            .AddDataProtection()
            .SetApplicationName("AlphaPlus")
            .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisConfiguration));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services;
    }
}
