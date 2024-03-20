using AlphaPlus.Settings.Site.Helpers;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .RegisterApplicationServices(configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

AlphaContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
AlphaPlus.Core.Configurations.ConfigurationManager.Init(configuration);
AlphaPlus.Configurations.ConfigurationManager.Init(configuration);

app.UsePathBase("/settings");
app.MapHealthChecks("/health");
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.UseMiddleware<RequestCultureMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SubAccount}/{action=Index}/{id?}");

app.Run();
