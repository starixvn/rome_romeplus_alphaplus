using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaPlus.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAlphaPlusCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IUserSession, UserSessionExt>();
            services.AddSingleton<IRequestAnalyst, RequestAnalyst>();
            services.AddSingleton<IBrowserHelper, BrowserHelper>();
            services.AddSingleton<IRequestContext, RequestContext>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();

            services.Configure<AuthorizationOptions>(configuration.GetSection(AuthorizationOptions.Authorization));

            return services;
        }
    }
}