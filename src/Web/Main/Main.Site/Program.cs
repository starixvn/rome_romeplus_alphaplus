using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Logging;
using AlphaPlus.Main.Site.Extensions;
using AlphaPlus.NetThrottling;
using AlphaPlus.SecurityCode.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Diagnostics.HealthChecks;

ThreadPool.SetMinThreads(100, 100);

const string PathBase = "/main";
const string SharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", SharedSettings);

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: false)
    .AddJsonFile(SharedSettings, optional: true, reloadOnChange: false)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"authorization.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"securitycode.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"bannerconfig.json", optional: false, reloadOnChange: false)
    .AddJsonFile("Domains/domainmappings.json", optional: true, reloadOnChange: false)
    //.AddEnvironmentVariables()
    .AddCommandLine(args);

AlphaPlus.Core.Configurations.ConfigurationManager.Init(builder.Configuration);
LoggingConfig.Init(["password", "seccode", "securitycode"]);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CheckBelongingFilterAttribute>();
    options.Filters.Add<AfterLoginFlowFilterAttribute>();
    options.Filters.Add<ProtectedModeFilterAttribute>();
    options.Filters.Add<SecurityCodeFilterAttribute>();
});

builder.Services
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    })
    .AddHttpContextAccessor()
    .AddApplicationServices(builder.Configuration)
    .AddNetThrottling(builder.Configuration)
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddSharedCookie();

var app = builder.Build();
app.UseForwardedHeaders();
app.UsePathBase(PathBase);
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    AllowCachingResponses = false,
    ResponseWriter = (context, report) =>
    {
        context.Response.ContentType = "text/plain";
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return context.Response.WriteAsync($"Healthy{Environment.NewLine}{environment}");
    },
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
}).WithMetadata(new AllowAnonymousAttribute(), new DisableNetThrottlingAttribute());

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RequestCultureMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
}

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = PathBase;

    if (context.Request.Cookies.ContainsKey(".AspNetCore.Session"))
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        
        if (context.User.Identity?.IsAuthenticated == true)
        {
            context.Response.Cookies.Append("X-Response-Session", context.Session.Id);
        }
    }

    await next(context);
});

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
StaticResources.Init(configuration.GetValue<string>("JsVersion"));

app.MapControllerRoute(name: "default", pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();