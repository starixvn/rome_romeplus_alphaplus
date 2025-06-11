namespace Alpha.Handbook.Demo.Helpers
{
    public static class ApplicationServicesHelper
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks();

            services
                .AddControllersWithViews(options => {})
                .AddNewtonsoftJson(option =>
                {
                    option.UseMemberCasing();
                })
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
                });

            services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly))
                .AddFanexLogging(configuration)
                .AddHttpContextAccessor();

            return services;
        }
    }
}