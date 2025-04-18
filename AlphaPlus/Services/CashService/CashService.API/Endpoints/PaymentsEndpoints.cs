using Microsoft.AspNetCore.Http.HttpResults;
using AlphaPlus.CashService.API.Services;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/payment-providers", async (IPaymentsService service) =>
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

        app.MapGet("/api/payment-provider-fees/{providerId}", async (int providerId, IPaymentsService service) =>
        {
            var fees = await service.GetPaymentProviderFees(providerId);
            return Results.Ok(fees);
        })
        .WithName("GetPaymentProviderFees")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment provider fees",
            Description = "Returns the fees for a specific payment provider"
        });
    }
} 