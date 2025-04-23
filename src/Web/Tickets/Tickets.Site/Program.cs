using System.Text.Json.Serialization;
using AlphaPlus.BetLists.Shared.Extensions;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.NetThrottling;
using AlphaPlus.RequestInfo;
using AlphaPlus.Results.Features.Shared;
using AlphaPlus.SecurityCode.Extensions;
using AlphaPlus.SecurityCode.Filters;
using AlphaPlus.Tickets.Site.Features.Shared.Extensions;
using Fanex.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;

ThreadPool.SetMinThreads(100, 100);
const string PathBase = "/tickets";

const string SharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", SharedSettings);

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: true) // When running using dotnet run
    .AddJsonFile(SharedSettings, optional: true, reloadOnChange: true) // When app is published or Docker
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("authorization.json", optional: false, reloadOnChange: true)
    .AddJsonFile("partialum.json", optional: false, reloadOnChange: true)
    .AddJsonFile("results.json", optional: false, reloadOnChange: true)
    .AddJsonFile("securitycode.json", optional: false, reloadOnChange: true)
    //.AddEnvironmentVariables()
    .AddCommandLine(args);

AlphaPlus.Core.Configurations.ConfigurationManager.Init(configuration);
StaticResources.Init(configuration.GetValue<string>("JsVersion"));

// Add services to the container.
builder.Services
    .Configure<MvcOptions>(options =>
    {
        options.MaxModelBindingCollectionSize = int.MaxValue;
    })
    .Configure<JsonOptions>(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
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
        // For BetLists feature
        options.ViewLocationFormats.Add("/Features/BetLists/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Features/BetLists/Views/Shared/{0}.cshtml");
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
    .AddSecurityCodeServices(configuration)
    .AddBetsForecastServices(configuration)
    .AddBetListsServices(configuration)
    .AddBetListsCore()
    .AddResultsServices(configuration)
    .AddResultsSiteServices(configuration)
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
    .UseAuthentication()
    .UseAuthorization()
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

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = PathBase;
    await next(context);
});

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
app.MapDefaultControllerRoute();
// app.MapControllerRoute(
//     "results",
//     "results/{controller}/{action}",
//     new { controller = "normal", action = "index" });

await app.RunAsync();
