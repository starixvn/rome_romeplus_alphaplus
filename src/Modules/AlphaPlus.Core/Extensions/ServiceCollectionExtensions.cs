using System.Diagnostics.CodeAnalysis;
using System.Net;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Services;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.AspNetCore.Filters;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using StackExchange.Redis;

namespace AlphaPlus.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAlphaPlusCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<IUserSession, UserSessionExt>();
        services.AddSingleton<IRequestAnalyst, RequestAnalyst>();
        services.AddSingleton<IBrowserHelper, BrowserHelper>();
        services.AddTransient<IRequestContext, RequestContext>();
        services.TryAddSingleton<INicknameChecker, NicknameChecker>();
        services.AddTransient<IAuthorizationService, AuthorizationService>();
        services.AddONEbookUM(configuration);
        services.AddControllersWithViews(options => options.Filters.Add<CheckUMFilterAttribute>());

        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });

        return services.Configure<AuthorizationOptions>(configuration.GetSection(AuthorizationOptions.Authorization));
    }

    public static IServiceCollection AddSharedSession(this IServiceCollection services, IConfiguration configuration)
    {
        var AppName = $"AlphaPlus_{configuration.GetValue<string>("SiteLabel")}";
        var redisConfiguration = configuration.GetValue<string>("RedisConfiguration");
        services
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfiguration;
                options.InstanceName = AppName;
            })
            .AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddDataProtection()
            .SetApplicationName(AppName)
            .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisConfiguration));

        return services;
    }

    public static AuthenticationBuilder AddSharedCookie(this AuthenticationBuilder builder)
        => builder.AddCookie(options =>
        {
            var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            var scheme = isDev ? "http" : "https";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
            options.AccessDeniedPath = "/Forbidden/";
            options.Cookie.Path = "/";
            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = ctx =>
                {
                    ctx.Response.Redirect($"{scheme}://{ctx.Request.Host.Value}/main/Account/Login?r=SessionExpired");
                    return Task.CompletedTask;
                }
            };
        });

    private static IServiceCollection AddONEbookUM(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new UMClientConfiguration().Initialize(configuration));
        services.AddSingleton<CheckUMFilterAttribute>();
        services.AddSingleton<IUMCacheService, MemoryUMCacheService>();
        services.AddSingleton<IUMRestClient, UMRestClient>();
        services.AddHttpContextAccessor();
        services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
        services.AddSingleton<IMaintenanceService, MaintenanceService>();

        return services;
    }

    public static IServiceCollection AddHttpClient<TClient, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        this IServiceCollection services,
        string baseUrl,
        int pooledConnectionLifetime = 15)
            where TClient : class
            where TImplementation : class, TClient
    {
        services
            .AddHttpClient<TClient, TImplementation>(httpClient => httpClient.BaseAddress = new Uri(baseUrl))
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            })
            .ConfigurePrimaryHttpMessageHandler(_ => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(pooledConnectionLifetime)
            })
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        return services;
    }
}