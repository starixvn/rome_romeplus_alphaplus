using System.Text.Json;
using AlphaPlus.CashService.Site.Helpers;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.RequestInfo;
using AlphaPlus.SecurityCode.Extensions;
using AlphaPlus.SecurityCode.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

const string pathBase = "/cash-service";
const string sharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", sharedSettings);

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: true)
    .AddJsonFile(sharedSettings, optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"authorization.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"securitycode.json", optional: false, reloadOnChange: true)
    //.AddEnvironmentVariables()
    .AddCommandLine(args);

AlphaPlus.Core.Configurations.ConfigurationManager.Init(builder.Configuration);

// Add services to the container.
builder.Services
    .AddRequestInfo()
    .AddFanexLogging(configuration)
    .AddAlphaPlusCore(configuration)
    .AddSharedSession(configuration)
    .AddSecurityCodeServices(configuration)
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
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
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddHttpClient<HttpClientHelper>();


builder.Services.AddHealthChecks();

builder.Services
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    })
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddSharedCookie();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseForwardedHeaders();
app
    .UsePathBase(pathBase)
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

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = pathBase;
    await next(context);
});

app.UseCors("AllowSpecificOrigins");

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
StaticResources.Init(configuration.GetValue<string>("JsVersion"));

app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Payments}/{action=Index}/{id?}");

try
{
    //Log.Information("Starting web application");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}