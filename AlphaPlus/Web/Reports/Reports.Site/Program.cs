using System.Text.Json.Serialization;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.NetThrottling;
using AlphaPlus.Reports.Site.Features.Shared.Extensions;
using AlphaPlus.RequestInfo;
using AlphaPlus.SecurityCode.Filters;
using Fanex.Data;
using Fanex.Logging;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;

const string App_Data = "App_Data";
var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(Path.Combine(App_Data, "connectionStrings.json"), optional: false, reloadOnChange: true)
    .AddJsonFile(Path.Combine(App_Data, "authorization.json"), optional: false, reloadOnChange: true)
    .AddJsonFile(Path.Combine(App_Data, "securitycode.json"), optional: false, reloadOnChange: true)
    .AddJsonFile(Path.Combine(App_Data, "partialum.json"), optional: false, reloadOnChange: true)
    //.AddEnvironmentVariables()
    .AddCommandLine(args);
AlphaPlus.Core.Configurations.ConfigurationManager.Init(configuration);

// Add services to the container.
builder.Services
    .Configure<JsonOptions>(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .AddControllersWithViews(options =>
    {
        options.Filters.Add<CheckBelongingFilterAttribute>();
        options.Filters.Add<AfterLoginFlowFilterAttribute>();
        options.Filters.Add<ProtectedModeFilterAttribute>();
        options.Filters.Add<SecurityCodeFilterAttribute>();
    })
    .AddRazorOptions(options =>
    {
        // The old locations are /Views/{1}/{0}.cshtml and /Views/Shared/{0}.cshtml
        // where {1} is the Controller and {0} is the name of the View
        options.ViewLocationFormats.Clear();
        options.ViewLocationFormats.Add("/Features/{1}/Views/{0}.cshtml");
        options.ViewLocationFormats.Add("/Features/Shared/Views/{0}.cshtml");
    });
builder.Services
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    })
    .AddMediatR(assembly)
    .AddFanexLogging(configuration)
    .AddSingleton<Fanex.Caching.ICacheService, Fanex.Caching.CacheService>()
    .AddRemotingRef(configuration)
    .AddFanexData(configuration)
    .AddRequestInfo()
    .AddAlphaPlusCore(configuration)
    .AddSharedSession(configuration)
    .AddApplicationServices(configuration)
    .AddNetThrottling(configuration)
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddSharedCookie();
builder.Services.AddHealthChecks();

var app = builder.Build();
// Configure the HTTP request pipeline.
app
    .UseForwardedHeaders()
    .UseExceptionHandler("/Shared/Error")
    .UsePathBase("/reports")
    .UseRouting()
    .UseDefaultFiles()
    .UseStaticFiles()
    .UseSession()
    .UseAuthentication()
    .UseAuthorization()
    .UseNetThrottling()
    .UseMiddleware<RequestCultureMiddleware>();
app.MapHealthChecks("/health");

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
StaticResources.Init(configuration.GetValue<string>("JsVersion"));

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = "/reports";

    await next(context);
});

app.MapDefaultControllerRoute();

await Logger.Log.InfoAsync($"{assembly.GetName().Name} has been started up.");
await app.RunAsync();
