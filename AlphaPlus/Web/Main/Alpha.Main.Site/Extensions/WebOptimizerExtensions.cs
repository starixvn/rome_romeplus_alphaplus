using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.V8;

namespace Alpha.Main.Site.Extensions;

public static class WebOptimizerExtensions
{
    public static IServiceCollection AddBundlingAndMinification(this IServiceCollection services)
    {
        services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName).AddV8();

        services.AddWebOptimizer(pipeline =>
        {
            pipeline.CompileTypeScriptFiles();
            pipeline.TranspileJavaScriptFiles();
            pipeline.AddJavaScriptBundle("/wwwroot/assets/bundles/default-index.min.js",
                "wwwroot/assets/scripts/home/assetsCDNHealthcheck.js",
                "wwwroot/assets/scripts/core.js",
                "wwwroot/assets/scripts/home/toast-message.js",
                "wwwroot/assets/scripts/home/index-manager.js",
                "wwwroot/assets/scripts/old-main.js",
                "wwwroot/assets/scripts/home/header-nav.js",
                "wwwroot/assets/scripts/home/header-msg.js",
                "wwwroot/assets/scripts/home/um-message.js",
                "wwwroot/assets/scripts/home/menu-bar.js",
                "wwwroot/assets/scripts/customer-profiles/brief-profile.js",
                "wwwroot/assets/scripts/dashboard/popup-announcement.js",
                "wwwroot/assets/scripts/advertisement/popup-advertisement.js");

            pipeline.AddJavaScriptBundle("/wwwroot/assets/bundles/suggest-telegram-otp.min.js",
                "wwwroot/assets/scripts/core.js",
                "wwwroot/assets/scripts/home/suggest-telegram-otp.js");

            pipeline.AddJavaScriptBundle("/wwwroot/assets/bundles/home/le-mtagconfig.min.js",
                "wwwroot/assets/scripts/home/le-mtagconfig.js");

            pipeline.AddScssBundle("/wwwroot/assets/bundles/default-index.min.css",
                "wwwroot/assets/styles/home/page.scss",
                "wwwroot/assets/styles/customer-profiles/customer-profile.scss",
                "wwwroot/assets/styles/home/dropdown-ext.scss",
                "wwwroot/assets/styles/home/popup-announcement.scss",
                "wwwroot/assets/styles/home/tooltip.scss",
                "wwwroot/assets/styles/home/popup-message.scss",
                "wwwroot/assets/styles/home/old-main.scss",
                "wwwroot/assets/styles/advertisement/popup-advertisement.scss");

            pipeline.AddScssBundle("/wwwroot/assets/bundles/page-mobile-calculate-07.min.css",
                "wwwroot/assets/styles/home/page-mobile-calculate-07.scss");

            pipeline.AddScssBundle("/wwwroot/assets/bundles/page-mobile-calculate-07-dark.min.css",
                "wwwroot/assets/styles/home/page-mobile-calculate-07-dark.scss");
        });

        return services;
    }
}
