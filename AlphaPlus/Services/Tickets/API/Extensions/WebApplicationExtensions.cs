using AlphaPlus.Tickets.API.Endpoints;
using AlphaPlus.Tickets.API.Middlewares;

namespace AlphaPlus.Tickets.API.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger().UseSwaggerUI();
        }

        app
            //.UseResponseCompression()
            .UseMiddleware<ExceptionMiddleware>();

        return app;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/health");

        typeof(IEndpoint).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IEndpoint)))
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>()
            .ToList()
            .ForEach(module => module.MapEndpoints(app));

        return app;
    }
}
