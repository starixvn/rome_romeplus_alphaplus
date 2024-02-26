using System.Reflection;
using AlphaPlus.Tickets.API.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AlphaPlus.Tickets.API.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        app.Services
            .GetRequiredService<IEnumerable<IEndpoint>>()
            .ToList()
            .ForEach(endpoint => endpoint.MapEndpoints(app));

        return app;
    }
}