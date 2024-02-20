using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using AlphaPlus.Settings.Site.SubAccount.Services;
using AlphaPlus.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(typeof(Program).Assembly);

var configuration = builder.Configuration;
builder.Services
    .AddFanexDataWithSqlServer(configuration)
    .AddFanexLogging(configuration)
    .AddRemotingRef(configuration)
    .AddHttpClient<ISubAccountService, SubAccountService>
        (httpClient => httpClient.BaseAddress = new Uri(configuration.GetValue<string>("CustomerSettingApi")));

builder.Services
    .AddAlphaPlusCore(configuration)
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddSharedCookie();

builder.Services
    .AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Clear();
        options.ViewLocationFormats.Add("/{1}/Views/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    });

builder.Services
    .AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UsePathBase("/settings");
app.MapHealthChecks("/health");
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SubAccount}/{action=Index}/{id?}/{custId=1176070}");

app.Run();
