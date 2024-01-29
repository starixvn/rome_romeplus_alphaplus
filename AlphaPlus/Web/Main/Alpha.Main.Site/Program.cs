using Alpha.Main.Site.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Logging;
using Fanex.Logging.Extensions.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

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

var redisConfiguration = builder.Configuration.GetValue<string>("RedisConfiguration");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConfiguration;
    options.InstanceName = "AlphaPlus";
});
builder.Services.AddSession();

builder.Services
    .AddDataProtection()
    .SetApplicationName("AlphaPlus")
    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisConfiguration));

builder.Services.AddControllersWithViews();
builder.Services
    .AddHttpContextAccessor()
    .AddApplicationServices(builder.Configuration)
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UsePathBase("/site-main");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

app.MapControllerRoute(name: "default", pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();