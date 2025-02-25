using AlphaPlus.AccountInfo.Site.Features.OpenDownlines;
using AlphaPlus.AccountInfo.Site.Features.ProfilePicture;
using AlphaPlus.AccountInfo.Site.Features.ProtectedStatus;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Filters;
using AlphaPlus.RequestInfo;
using AlphaPlus.SecurityCode.Extensions;
using AlphaPlus.SecurityCode.Filters;
using Fanex.Caching;
using Fanex.Data;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AlphaPlus.AccountInfo.Site.Extensions;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR()
            .AddFanexData(configuration)
            .AddFanexLogging(configuration)
            .AddRequestInfo();

        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton(Fanex.Logging.Logger.Log);

        services
            .AddAlphaPlusCore(configuration)
            .AddSharedSession(configuration)
            .AddSecurityCodeServices(configuration);

        services.AddSingleton<Utilities.IP.IPService, Utilities.IP.IPServiceImpl>();
        services.AddSingleton<IKickOutCustomerService, KickOutCustomerService>();
        services.AddHttpClient<ISearchAccountService, SearchAccountService>(httpClient => httpClient.BaseAddress = new Uri(configuration.GetValue<string>("Fanex.Customer:ServiceUri")));
        services.AddSingleton<IProfilePictureService, ProfilePictureService>();
        services.AddSingleton<IUnsuspendDownlinesService, UnsuspendDownlinesService>();
        services.AddSingleton<OpenDownlinesService, OpenDownlinesServiceImpl>();
        services.AddSingleton<IProtectedStatusService, ProtectedStatusService>();

        services
        .AddControllersWithViews(options =>
        {
            options.Filters.Add<CheckBelongingFilterAttribute>();
            options.Filters.Add<AfterLoginFlowFilterAttribute>();
            options.Filters.Add<ProtectedModeFilterAttribute>();
            options.Filters.Add<SecurityCodeFilterAttribute>();
        }).AddNewtonsoftJson(option =>
            {
                option.UseMemberCasing();
            })
            .AddRazorOptions(options =>
        {
            options.ViewLocationFormats.Clear();
            options.ViewLocationFormats.Add("/{1}/Views/{0}.cshtml");
            options.ViewLocationFormats.Add("/Features/{1}/Views/{0}.cshtml");
            options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
        });

        services.AddHealthChecks();

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