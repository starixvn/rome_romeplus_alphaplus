using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Features.Site.Helpers;
using AlphaPlus.NetThrottling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

ThreadPool.SetMinThreads(100, 100);

const string PathBase = "/features";
const string SharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", SharedSettings);

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: true)
    .AddJsonFile(SharedSettings, optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("authorization.json", optional: false, reloadOnChange: true);

builder.Services
    .RegisterApplicationServices(configuration)
    .AddNetThrottling(configuration)
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
AlphaPlus.Core.Configurations.ConfigurationManager.Init(configuration);
StaticResources.Init(configuration.GetValue<string>("JsVersion"));

app.UseForwardedHeaders();
app.UsePathBase(PathBase);
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute(), new DisableNetThrottlingAttribute());
app.UseStaticFiles(new StaticFileOptions());
app.UseRouting();
app.UseAuthorization();
app.UseSession();
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

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = PathBase;

    if (context.Request.Cookies.ContainsKey(".AspNetCore.Session"))
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    }

    await next(context);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=NewMessage}/{action=GetNumberOfNewAnnouncement}/{id?}");

app.Run();
