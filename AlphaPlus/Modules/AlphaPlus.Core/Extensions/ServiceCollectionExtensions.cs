using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AlphaPlus.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAlphaPlusCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IUserSession, UserSessionExt>();
        services.AddSingleton<IRequestAnalyst, RequestAnalyst>();
        services.AddSingleton<IBrowserHelper, BrowserHelper>();
        services.AddSingleton<IRequestContext, RequestContext>();
        services.AddSingleton<IAuthorizationService, AuthorizationService>();

        return services.Configure<AuthorizationOptions>(configuration.GetSection(AuthorizationOptions.Authorization));
    }

    public static IServiceCollection AddSharedSession(this IServiceCollection services, IConfiguration configuration)
    {
        const string AppName = "AlphaPlus";
        var redisConfiguration = configuration.GetValue<string>("RedisConfiguration");
        services
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfiguration;
                options.InstanceName = AppName;
            })
            .AddSession()
            .AddDataProtection()
            .SetApplicationName(AppName)
            .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisConfiguration));

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