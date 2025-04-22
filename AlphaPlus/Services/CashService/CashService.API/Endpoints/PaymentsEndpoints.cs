using Microsoft.AspNetCore.Http.HttpResults;
using AlphaPlus.CashService.API.Services;
using AlphaPlus.CashService.API.Models.Payments;
using Newtonsoft.Json;

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

        app.MapGet("/api/payment-providers/{providerId}", async (int providerId, IPaymentsService service) =>
        {
            var provider = await service.GetPaymentProviderById(providerId);
            if (provider == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(provider);
        })
        .WithName("GetPaymentProviderById")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment provider by ID",
            Description = "Returns a specific payment provider by its ID"
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

        app.MapPost("/api/set-daily-withdrawal-count", async (int custId, int count, IPaymentsService service) =>
        {
            var result = await service.SetDailyWithdrawalCount(custId, count);
            return Results.Ok(result);
        })
        .WithName("SetDailyWithdrawalCount")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Set daily withdrawal count",
            Description = "Sets the daily withdrawal count for a specific customer"
        });

        app.MapGet("/api/payment-provider-settings", async (int providerId, int method, int status, int transactionType, IPaymentsService service) =>
        {
            var settings = await service.GetPaymentProviderSettings(providerId, method, status, transactionType);
            return Results.Ok(settings);
        })
        .WithName("GetPaymentProviderSettings")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment provider settings",
            Description = "Returns the settings for a specific payment provider based on method, status, and transaction type"
        });

        app.MapPut("/api/update-payment-provider-settings", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var settings = JsonConvert.DeserializeObject<IEnumerable<ProviderSettingsModel>>(body);

            var result = await service.UpdatePaymentProviderSettings(settings);
            return Results.Ok(result);
        })
        .WithName("UpdatePaymentProviderSettings")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update payment provider settings",
            Description = "Updates the settings for a payment provider"
        });
        
        app.MapGet("/api/payment-methods", async (IPaymentsService service) =>
        {
            var methods = await service.GetAllPaymentMethods();
            return Results.Ok(methods);
        })
        .WithName("GetAllPaymentMethods")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get all payment methods",
            Description = "Returns a list of all available payment methods"
        });

        app.MapPost("/api/add-payment-method", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var method = JsonConvert.DeserializeObject<PaymentMethodModel>(body);
            
            if (method == null)
            {
                return Results.BadRequest("Invalid payment method data");
            }

            var result = await service.InsertPaymentMethod(method);
            return Results.Ok(result);
        })
        .WithName("AddPaymentMethod")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Add payment method",
            Description = "Adds a new payment method to the system"
        });
    }
} 