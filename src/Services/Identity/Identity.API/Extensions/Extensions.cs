using AlphaPlus.Identity.API.Features.Auth;
using AlphaPlus.Identity.API.Features.Auth.SiteDb;
using AlphaPlus.Identity.API.Utils;
using AlphaPlus.Identity.Features.Authorization;

namespace AlphaPlus.Identity.API.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IEncryption, Encryption>()
                .AddSingleton<ISiteDBService, SiteDBService>()
                .AddSingleton<IAdminPermissionService, AdminPermissionService>()
                .AddSingleton<IAuthorizationService, AuthorizationService>()
                .AddHttpClient<IAuthenticateService, AuthenticateService>();

            return services;
        }
    }
}