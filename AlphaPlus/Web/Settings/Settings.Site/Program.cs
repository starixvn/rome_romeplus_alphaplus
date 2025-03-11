using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

ThreadPool.SetMinThreads(100, 100);

var builder = WebApplication.CreateBuilder(args);
const string SharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", SharedSettings);

builder.Configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: true)
    .AddJsonFile(SharedSettings, optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Features/ProductConfigs/ProductConfiguration.json", optional: false, reloadOnChange: true)
    .AddJsonFile("authorization.json", optional: false, reloadOnChange: true)
    .AddJsonFile("securitycode.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Products.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Domains/domainmappings.json", optional: true, reloadOnChange: true)
    //.AddEnvironmentVariables()
    .AddCommandLine(args);

var configuration = builder.Configuration;

builder.Services
    .RegisterApplicationServices(configuration)
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
}

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
AlphaPlus.Core.Configurations.ConfigurationManager.Init(configuration);
StaticResources.Init(configuration.GetValue<string>("JsVersion"));

app.UseForwardedHeaders();
app.UsePathBase("/settings");
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.UseMiddleware<RequestCultureMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SubAccount}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = "/settings";

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

app.Run();