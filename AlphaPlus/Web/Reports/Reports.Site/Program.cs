using System.Text.Json.Serialization;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Core.Models;
using AlphaPlus.NetThrottling;
using AlphaPlus.Reports.Site.Features.Shared.Extensions;
using AlphaPlus.RequestInfo;
using AlphaPlus.SecurityCode.Filters;
using Fanex.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;

ThreadPool.SetMinThreads(100, 100);

const string SharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", SharedSettings);

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: true) // When running using dotnet run
    .AddJsonFile(SharedSettings, optional: true, reloadOnChange: true) // When app is published or Docker
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("authorization.json", optional: false, reloadOnChange: true)
    .AddJsonFile("securitycode.json", optional: false, reloadOnChange: true)
    .AddJsonFile("partialum.json", optional: false, reloadOnChange: true)
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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
}

app
    .UsePathBase("/reports")
    .UseRouting()
    .UseDefaultFiles()
    .UseStaticFiles()
    .UseSession()
    .UseAuthentication()
    .UseAuthorization()
    .UseNetThrottling()
    .UseMiddleware<RequestCultureMiddleware>();

app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute(), new DisableNetThrottlingAttribute());

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
StaticResources.Init(configuration.GetValue<string>("JsVersion"));

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = "/reports";

    var session = context.RequestServices.GetRequiredService<IUserSession>();
    context.Items["customData"] = new
    {
        session.CustId,
        session.UserName,
        session.RoleId,
        session.SubAccId,
        session.SubUserName,
        session.AdminId,
        session.AdminName
    };

    await next(context);
});

app.MapDefaultControllerRoute();

// await Logger.Log.InfoAsync($"{assembly.GetName().Name} has been started up.");
await app.RunAsync();
