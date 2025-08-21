using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.NetThrottling;
using AlphaPlus.Settings.Site.Features.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

ThreadPool.SetMinThreads(100, 100);

var builder = WebApplication.CreateBuilder(args);
const string PathBase = "/settings";
const string SharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", SharedSettings);

builder.Configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: false)
    .AddJsonFile(SharedSettings, optional: true, reloadOnChange: false)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("Features/ProductConfigs/ProductConfiguration.json", optional: false, reloadOnChange: false)
    .AddJsonFile("authorization.json", optional: false, reloadOnChange: false)
    .AddJsonFile("securitycode.json", optional: false, reloadOnChange: false)
    .AddJsonFile("Products.json", optional: false, reloadOnChange: false)
    .AddJsonFile("Domains/domainmappings.json", optional: true, reloadOnChange: false)
    //.AddEnvironmentVariables()
    .AddCommandLine(args);

var configuration = builder.Configuration;

builder.Services
    .RegisterApplicationServices(configuration)
    .AddNetThrottling(configuration)
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

var app = builder.Build();

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
AlphaPlus.Core.Configurations.ConfigurationManager.Init(configuration);
StaticResources.Init(configuration.GetValue<string>("JsVersion"));

app.UseForwardedHeaders();
app.UsePathBase(PathBase);
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute(), new DisableNetThrottlingAttribute());
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.UseSessionCookieComparison();
app.UseNetThrottling();
app.UseMiddleware<RequestCultureMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SubAccount}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = PathBase;

    if (context.Request.Cookies.ContainsKey(".AspNetCore.Session"))
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        context.Response.Cookies.Append("X-Response-Session", context.Request.Cookies[".AspNetCore.Session"]);
    }

    await next(context);
});

app.Run();