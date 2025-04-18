using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using AlphaPlus.CashService.API.Services;
using AlphaPlus.CashService.API.Models.ManagePayments;

namespace AlphaPlus.CashService.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseCashServiceSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CashService API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        return app;
    }

    public static WebApplication UseCashServiceExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;

                var result = JsonSerializer.Serialize(new
                {
                    error = exception?.Message ?? "An error occurred",
                    statusCode = StatusCodes.Status500InternalServerError
                });

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(result);
            });
        });

        return app;
    }

    public static WebApplication UseCashServiceCors(this WebApplication app)
    {
        app.UseCors("AllowSpecificOrigins");
        return app;
    }

    public static WebApplication MapAPIEndpoints(this WebApplication app)
    {
        app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
            .WithName("HealthCheck")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Health check endpoint",
                Description = "Returns the health status of the API"
            });

        app.MapGet("/version", () => Results.Ok(new { Version = "1.0.0", Environment = app.Environment.EnvironmentName }))
            .WithName("Version")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Version information",
                Description = "Returns the current version of the API"
            });

        app.MapGet("/api/payment-providers", async (IManagePaymentsService service) =>
        {
            var providers = await service.GetAllPaymentProviders();
            return Results.Ok(providers);
        })
        .WithName("GetPaymentProviders")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get all payment providers",
            Description = "Returns a list of all available payment providers"
        });

        app.MapGet("/api/payment-provider-fees", async (IManagePaymentsService service) =>
        {
            var fees = await service.GetPaymentProviderFees();
            return Results.Ok(fees);
        })
        .WithName("GetPaymentProviderFees")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment provider fees",
            Description = "Returns a list of all payment provider fees"
        });

        return app;
    }
} 