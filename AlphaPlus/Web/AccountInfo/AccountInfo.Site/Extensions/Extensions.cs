using AlphaPlus.AccountInfo.Site.Features.SearchAccount;
using AlphaPlus.Core.Extensions;
using AlphaPlus.RequestInfo;
using AlphaPlus.SecurityCode.Services;
using AlphaPlus.SecurityCode.Web;
using Fanex.Caching;
using Fanex.Data;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AlphaPlus.AccountInfo.Site.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMediatR()
                .AddFanexData(configuration)
                .AddRequestInfo();

            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IDynamicRepository, DynamicRepository>();
            services.AddSingleton(Fanex.Logging.Logger.Log);

            services.AddAlphaPlusCore(configuration).AddSharedSession(configuration);

            services.AddSingleton<ISecurityCodePathBuilder, SecurityCodePathBuilder>();
            services.AddSingleton<ISecurityCodeHttpContext, SecurityCodeHttpContext>();
            services.AddSingleton<ISecurityCodeChecker, SecurityCodeChecker>();

            services.AddSingleton<Utilities.IP.IPService, Utilities.IP.IPServiceImpl>();
            services.AddSingleton<ISearchAccountService, SearchAccountService>();


            // services.Configure<SecurityCodeOptions>(configuration.GetSection(SecurityCodeOptions.SecurityCode));

            return services;
        }

        private static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
            return services;
        }

        public static AuthenticationBuilder AddSharedCookie(this AuthenticationBuilder builder)
        => builder.AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
            options.AccessDeniedPath = "/Forbidden/";
            options.Cookie.Path = "/";
            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = ctx =>
                {
                    ctx.Response.Redirect($"{ctx.Request.Scheme}://{ctx.Request.Host.Value}/main/Account/Login");
                    return Task.CompletedTask;
                }
            };
        });
    }
}