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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;

ThreadPool.SetMinThreads(100, 100);
const string PathBase = "/reports";

const string SharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", SharedSettings);

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: false) // When running using dotnet run
    .AddJsonFile(SharedSettings, optional: true, reloadOnChange: false) // When app is published or Docker
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("authorization.json", optional: false, reloadOnChange: false)
    .AddJsonFile("securitycode.json", optional: false, reloadOnChange: false)
    .AddJsonFile("partialum.json", optional: false, reloadOnChange: false)
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
    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly))
    .AddFanexLogging(configuration)
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
app.UseForwardedHeaders();
app
    .UsePathBase(PathBase)
    .UseRouting()
    .UseDefaultFiles()
    .UseStaticFiles()
    .UseSession()
    .UseSessionCookieComparison()
    .UseAuthentication()
    .UseAuthorization()
    .UseNetThrottling()
    .UseMiddleware<RequestCultureMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
}

app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute(), new DisableNetThrottlingAttribute());

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
StaticResources.Init(configuration.GetValue<string>("JsVersion"));

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = PathBase;

    if (context.Request.Cookies.ContainsKey(".AspNetCore.Session"))
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        context.Response.Cookies.Append("X-Response-Session", context.Session.Id, new CookieOptions
        {
            HttpOnly = false,
            Path = "/"
        });
    }

    await next(context);
});

app.MapDefaultControllerRoute();

await app.RunAsync();
