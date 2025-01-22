using AlphaPlus.AccountInfo.Site.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Core.Models;
using AlphaPlus.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

const string SharedSettings = "sharedSettings.json";
var sharedSettingsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "Shared/Configuration/", SharedSettings);

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration
    .AddJsonFile(sharedSettingsPath, optional: true, reloadOnChange: true)
    .AddJsonFile(SharedSettings, optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"authorization.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"securitycode.json", optional: false, reloadOnChange: true)
    //.AddEnvironmentVariables()
    .AddCommandLine(args);

AlphaPlus.Core.Configurations.ConfigurationManager.Init(builder.Configuration);
LoggingConfig.Init(["password", "seccode", "securitycode"]);

builder.Services
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    })
    .AddHttpContextAccessor()
    .AddApplicationServices(builder.Configuration)
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddSharedCookie();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
}

app.UseForwardedHeaders();
app.UsePathBase("/account-info");
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestCultureMiddleware>();

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    context.Request.PathBase = "/account-info";

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

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
StaticResources.Init(app.Configuration.GetValue<string>("JsVersion"));

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");


app.Run();
