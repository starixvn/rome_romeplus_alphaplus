using AlphaPlus.Tickets.API.Endpoints;
using AlphaPlus.Tickets.API.Middlewares;

namespace AlphaPlus.Tickets.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services
            .AddFanexLogging(configuration)
            .AddFanexDataWithSqlServer(configuration)
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddHealthChecks();
    }

    public static WebApplication RegisterMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger().UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }

    public static WebApplication RegisterEndpoints(this WebApplication app, IServiceCollection services)
    {
        app.MapHealthChecks("/health");

        typeof(IEndpoints).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IEndpoints)))
            .Select(Activator.CreateInstance)
            .Cast<IEndpoints>()
            .ToList()
            .ForEach(module =>
            {
                module.RegisterServices(services);
                module.RegisterEndpoints(app);
            });

        return app;
    }
}
