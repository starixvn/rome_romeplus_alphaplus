using Alpha.Main.Site.Extensions;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Logging;
using AlphaPlus.SecurityCode.Filters;
using Fanex.Logging.Extensions.Database;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"authorization.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"securitycode.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

AlphaPlus.Core.Configurations.ConfigurationManager.Init(builder.Configuration);
LoggingConfig.Init(new[] { "password", "seccode", "securitycode" }, DatabaseInfoGetters.GetDbExceptionInfo);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CheckBelongingFilterAttribute>();
    options.Filters.Add<AfterLoginFlowFilterAttribute>();
    options.Filters.Add<ProtectedModeFilterAttribute>();
    options.Filters.Add<SecurityCodeFilterAttribute>();
});

builder.Services
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

app.UsePathBase("/main");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestCultureMiddleware>();

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

app.MapControllerRoute(name: "default", pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();