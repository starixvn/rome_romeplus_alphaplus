using AlphaPlus.NetThrottling.Filters;
using AlphaPlus.NetThrottling.Resolvers;
using AlphaPlus.NetThrottling.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaPlus.NetThrottling
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNetThrottling(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<NetThrottlingConfiguration>(configuration.GetSection("AlphaPlus.NetThrottling"));
            services.AddSingleton<IClock, Clock>();
            services.AddSingleton<ICacheServiceFactory, CacheServiceFactory>();
            services.AddSingleton<IFilterFactory, FilterFactory>();
            services.AddSingleton<IResolverFactory, ResolverFactory>();
            services.AddSingleton<IpFilter>()
                .AddSingleton<IFilter, IpFilter>(s => s.GetService<IpFilter>());
            services.AddSingleton<SessionFilter>()
                .AddSingleton<IFilter, SessionFilter>(s => s.GetService<SessionFilter>());
            services.AddSingleton<WhitelistSessionFilter>()
                .AddSingleton<IFilter, WhitelistSessionFilter>(s => s.GetService<WhitelistSessionFilter>());
            services.AddSingleton<INetThrottler, NetThrottler>();

            return services;
        }
    }
}