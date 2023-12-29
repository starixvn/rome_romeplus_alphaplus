using AlphaPlus.Tickets.Site.Extensions;
using AlphaPlus.Core.Helpers;
using Fanex.Logging;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;

var assembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
    .AddFanexLogging(configuration)
    .AddSharedSession(configuration)
    .AddApplicationServices()
    .AddMediatR(assembly)
    .AddHealthChecks();
builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    });
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    //.UseDefaultFiles()
    .UseStaticFiles()
    .UseRouting()
    .UseSession();
//.UseAuthentication()
//.UseAuthorization();

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

app.MapHealthChecks("/health");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await Logger.Log.InfoAsync($"{assembly.GetName().Name} has been started up.");
await app.RunAsync();
