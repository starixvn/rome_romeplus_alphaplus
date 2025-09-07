using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace AlphaPlus.NetThrottling.Extensions
{
    public static class RateLimiterExtensions
    {
        /// <summary>
        /// Adds AlphaPlus Rate Limiter services to the service collection
        /// </summary>
        public static IServiceCollection AddAlphaPlusRateLimiter(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure rate limiter options from configuration
            services.Configure<RateLimiterConfiguration>(configuration.GetSection("AlphaPlus.NetThrottling"));
            
            return services;
        }

        /// <summary>
        /// Adds AlphaPlus Rate Limiter services with custom configuration
        /// </summary>
        public static IServiceCollection AddAlphaPlusRateLimiter(this IServiceCollection services, Action<RateLimiterConfiguration> configureOptions)
        {
            services.Configure(configureOptions);
            return services;
        }

        /// <summary>
        /// Uses AlphaPlus Rate Limiter middleware in the request pipeline
        /// </summary>
        public static IApplicationBuilder UseAlphaPlusRateLimiter(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RateLimiterMiddleware>();
        }

        /// <summary>
        /// Creates default configuration matching the original NetThrottling settings
        /// </summary>
        public static RateLimiterConfiguration CreateDefaultConfiguration()
        {
            return new RateLimiterConfiguration
            {
                AnyPolicy = new PolicyConfiguration
                {
                    MaxRequests = 15,
                    ThrottleDuration = 30,
                    BlockDuration = 60
                },
                MainPolicy = new PolicyConfiguration
                {
                    MaxRequests = 60,
                    ThrottleDuration = 120,
                    BlockDuration = 600
                },
                NewCustomerPolicy = new PolicyConfiguration
                {
                    MaxRequests = 10,
                    ThrottleDuration = 600,
                    BlockDuration = 1800
                },
                WhiteListCustIds = new[] { 17996464, 21984259, 20269385, 19885952, 20171473 }
            };
        }
    }
}
