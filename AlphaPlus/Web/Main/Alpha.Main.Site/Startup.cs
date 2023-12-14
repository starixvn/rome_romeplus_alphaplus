using Alpha.Main.Site.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Logging;
using Fanex.Logging.Extensions.Database;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Alpha.Main.Site;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        AlphaPlus.Core.Configurations.ConfigurationManager.Init(Configuration);
        LoggingConfig.Init(new[] { "password", "seccode", "securitycode" }, DatabaseInfoGetters.GetDbExceptionInfo);

        services.AddControllersWithViews();
        services.AddSession()
            .AddHttpContextAccessor()
            .AddApplicationServices(Configuration)
            .AddAuthorization()
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();

        AlphaContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("default", "{controller=Default}/{action=Index}/{id?}");
            endpoints.MapControllers();
        });
    }
}
