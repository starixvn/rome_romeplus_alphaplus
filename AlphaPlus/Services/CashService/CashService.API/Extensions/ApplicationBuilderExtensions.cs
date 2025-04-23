using System.Text.Json;
using AlphaPlus.CashService.API.Endpoints;
using Microsoft.AspNetCore.Diagnostics;

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
        app.MapSystemEndpoints();
        app.MapPaymentsEndpoints();
        app.MapMemberSiteManagementEndpoints();
        app.MapPendingDepositEndpoints();

        return app;
    }
}