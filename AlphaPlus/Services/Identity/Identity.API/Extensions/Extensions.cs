using AlphaPlus.Identity.API.Features.Auth;
using AlphaPlus.Identity.API.Utils;

namespace AlphaPlus.Identity.API.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IEncryption, Encryption>()
                .AddSingleton<IAuthenticateService, AuthenticateService>();

            return services;
        }
    }
}