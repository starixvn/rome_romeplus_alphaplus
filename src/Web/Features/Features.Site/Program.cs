using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Features.Site.Helpers;
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
app.MapHealthChecks("/health");
app.UseStaticFiles(new StaticFileOptions());
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.UseMiddleware<RequestCultureMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
}

app.UseDeveloperExceptionPage(); // For development only - remove in production

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = PathBase;
    await next(context);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=NewMessage}/{action=GetNumberOfNewAnnouncement}/{id?}");

app.Run();
