using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Core.Models;
using AlphaPlus.Features.Site.Helpers;
using Microsoft.AspNetCore.HttpOverrides;

const string SharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", SharedSettings);

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: true)
    .AddJsonFile(SharedSettings, optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services
    .RegisterApplicationServices(configuration)
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
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
app.UsePathBase("/features");
app.MapHealthChecks("/health");
app.UseStaticFiles(new StaticFileOptions());

app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.UseMiddleware<RequestCultureMiddleware>();

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = "/features";

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=NewMessage}/{action=GetNumberOfNewAnnouncement}/{id?}");

app.Run();
