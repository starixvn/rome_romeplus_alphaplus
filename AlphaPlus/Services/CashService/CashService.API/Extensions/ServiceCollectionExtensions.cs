using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AlphaPlus.CashService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCashServiceApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CashService API",
                Version = "v1",
                Description = "A minimal API for handling cash-related operations",
                Contact = new OpenApiContact
                {
                    Name = "API Support",
                    Email = "support@cashservice.com"
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddCashServiceCors(this IServiceCollection services, string[] allowedOrigins)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins",
                policy => policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        return services;
    }

    public static IServiceCollection AddCashServiceLogging(this IServiceCollection services)
    {
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
        });

        return services;
    }
} 