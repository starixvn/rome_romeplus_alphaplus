using SiteServices = AlphaPlus.Settings.Site.SubAccount.Services;
using InfraServices = AlphaPlus.Settings.SubAccount.Infrastruture;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using AlphaPlus.Settings.Site.SubAccount.Services;

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
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    });

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SubAccount}/{action=Index}/{id?}/{custId=1176070}");

app.Run();
