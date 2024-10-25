using AlphaPlus.Identity.API.Features.Auth;
using AlphaPlus.Identity.API.Features.Auth.SiteDb;
using AlphaPlus.Identity.API.Utils;

namespace AlphaPlus.Identity.API.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IEncryption, Encryption>()
                .AddSingleton<ISiteDBService, SiteDBService>()
                .AddSingleton<IAuthenticateService, AuthenticateService>()
                .AddSingleton<IAdminPermissionService, AdminPermissionService>();

            return services;
        }
    }
}