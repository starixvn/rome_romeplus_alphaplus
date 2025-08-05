using System.Text.Json;
using AlphaPlus.CashService.Site.Features.ChatHub;
using AlphaPlus.CashService.Site.Features.Promotion.Services;
using AlphaPlus.CashService.Site.Features.Transactions.Services;
using AlphaPlus.CashService.Site.Helpers;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.NetThrottling;
using AlphaPlus.RequestInfo;
using AlphaPlus.SecurityCode.Extensions;
using AlphaPlus.SecurityCode.Filters;
using Fanex.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

const string pathBase = "/cash-service";
const string sharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", sharedSettings);

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: false)
    .AddJsonFile(sharedSettings, optional: true, reloadOnChange: false)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"authorization.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"securitycode.json", optional: false, reloadOnChange: false)
    .AddCommandLine(args);

AlphaPlus.Core.Configurations.ConfigurationManager.Init(builder.Configuration);

// Add services to the container.
builder.Services
    .AddFanexData(configuration)
    .AddRequestInfo()
    .AddFanexLogging(configuration)
    .AddAlphaPlusCore(configuration)
    .AddNetThrottling(configuration)
    .AddSharedSession(configuration)
    .AddSecurityCodeServices(configuration)
    .AddDependencyInjection(configuration)
    .AddLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.AddDebug();
    })
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
builder.Services.AddSignalR();

builder.Services
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    })
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddSharedCookie();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "CSRF-TOKEN";
});

builder.Services.AddTransient<ITransactionHistoryWorkbook, TransactionHistoryWorkbook>();
builder.Services.AddTransient<IPendingPromotionReportWorkbook, PendingPromotionReportWorkbook>();
builder.Services.AddScoped<IPromotionReportWorkbook, PromotionReportWorkbook>();
builder.Services.AddScoped<IPromotionDetailsReportWorkbook, PromotionDetailsReportWorkbook>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 125_829_120; // 500MB
});

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

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = pathBase;
    var maxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
    if (maxRequestBodySizeFeature != null)
    {
        maxRequestBodySizeFeature.MaxRequestBodySize = 125_829_120; // 120MB
    }

    if (context.Request.Cookies.ContainsKey(".AspNetCore.Session"))
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    }

    await next(context);
});

app.UseCors("AllowSpecificOrigins");

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
StaticResources.Init(configuration.GetValue<string>("JsVersion"));

app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute(), new DisableNetThrottlingAttribute());
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Payments}/{action=Index}/{id?}");

// Add SignalR hub endpoint
app.MapHub<ChatHub>("/chathub");
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