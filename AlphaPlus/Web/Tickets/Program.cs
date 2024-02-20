using System.Text.Json.Serialization;
using AlphaPlus.BetLists.Extensions;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Tickets.Site.Features.Shared.Extensions;
using Fanex.Logging;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

var assembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
AlphaPlus.Core.Configurations.ConfigurationManager.Init(builder.Configuration);
builder.Services
    .AddFanexLogging(configuration)
    .AddRemotingRef(configuration)
    .AddAlphaPlusCore(configuration)
    .AddBetListsServices()
    .AddApplicationServices(configuration)
    .AddMediatR(assembly)
    .AddHealthChecks();
builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddSharedCookie();
builder.Services
    .Configure<JsonOptions>(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        // The old locations are /Views/{1}/{0}.cshtml and /Views/Shared/{0}.cshtml
        // where {1} is the Controller and {0} is the name of the View
        options.ViewLocationFormats.Clear();
        options.ViewLocationFormats.Add("/Features/{1}/Views/{0}.cshtml");
        options.ViewLocationFormats.Add("/Features/Shared/Views/{0}.cshtml");
        // For BetLists feature
        options.ViewLocationFormats.Add("/Features/BetLists/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Features/BetLists/Views/Shared/{0}.cshtml");
    });

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app
    .UsePathBase("/tickets")
    .UseDefaultFiles()
    .UseStaticFiles()
    .UseRouting()
    .UseSession()
    .UseAuthentication()
    .UseAuthorization();

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

app.UseMiddleware<RequestCultureMiddleware>();
app.MapHealthChecks("/health");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await Logger.Log.InfoAsync($"{assembly.GetName().Name} has been started up.");
await app.RunAsync();
