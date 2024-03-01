using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Core.Helpers;
using Fanex.Data;
using AlphaPlus.RequestInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
AlphaPlus.Core.Configurations.ConfigurationManager.Init(builder.Configuration);

// TODO:
builder.Services.AddControllersWithViews(options =>
{
    // options.Filters.Add<CheckBelongingFilterAttribute>();
    // options.Filters.Add<AfterLoginFlowFilterAttribute>();
    // options.Filters.Add<ProtectedModeFilterAttribute>();
});

builder.Services
    .AddHttpContextAccessor()
    .AddFanexData(builder.Configuration)
    .AddRequestInfo()
    .AddAlphaPlusCore(builder.Configuration)
    .AddSharedSession(builder.Configuration)
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddSharedCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UsePathBase("/account");
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestCultureMiddleware>();

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
