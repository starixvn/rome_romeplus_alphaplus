using System.Text.Json.Serialization;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Tickets.Site.Features.Shared.Extensions;
using AlphaPlus.RequestInfo;
using Fanex.Logging;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Fanex.Data;
using AlphaPlus.Results.Features.Shared;
using AlphaPlus.BetLists.Shared.Extensions;
using AlphaPlus.Core.Filters;
using AlphaPlus.SecurityCode.Filters;
using AlphaPlus.SecurityCode.Extensions;

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("authorization.json", optional: false, reloadOnChange: true)
    .AddJsonFile("partialum.json", optional: false, reloadOnChange: true)
    .AddJsonFile("securitycode.json", optional: false, reloadOnChange: true)
    //.AddEnvironmentVariables()
    .AddCommandLine(args);
AlphaPlus.Core.Configurations.ConfigurationManager.Init(configuration);

// Add services to the container.
builder.Services
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
    .AddMediatR(assembly)
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
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddSharedCookie();
builder.Services.AddHealthChecks();

var app = builder.Build();
// Configure the HTTP request pipeline.
app
    .UseExceptionHandler("/Shared/Error")
    .UsePathBase("/tickets")
    .UseRouting()
    .UseDefaultFiles()
    .UseStaticFiles()
    .UseSession()
    .UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<RequestCultureMiddleware>();
app.MapHealthChecks("/health");

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
app.MapDefaultControllerRoute();
// app.MapControllerRoute(
//     "results",
//     "results/{controller}/{action}",
//     new { controller = "normal", action = "index" });

await Logger.Log.InfoAsync($"{assembly.GetName().Name} has been started up.");
await app.RunAsync();
