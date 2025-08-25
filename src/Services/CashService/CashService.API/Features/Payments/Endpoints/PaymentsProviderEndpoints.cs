using AlphaPlus.CashService.API.Features.Payments.Requests;
using Newtonsoft.Json;
using AlphaPlus.CashService.API.Features.Payments.Services;

namespace AlphaPlus.CashService.API.Features.Payments.Endpoints;

public static class PaymentsProviderEndpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapGet("/api/payments/providers-by-custid/{custId}", async (int custId, string languageCode, IPaymentsService service) =>
        {
            var providers = await service.GetProviders(custId, languageCode);
            return Results.Ok(providers);
        })
        .WithName("GetPaymentProviders")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment providers",
            Description = "Returns a list of available payment providers"
        });

        app.MapGet("/api/payments/providers/{custId}/{providerId}", async (int custId, int providerId, string languageCode, IPaymentsService service) =>
        {
            var provider = await service.GetProviderById(custId, providerId, languageCode);
            return Results.Ok(provider);
        })
        .WithName("GetPaymentProviderById")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment provider by ID",
            Description = "Returns a specific payment provider by its ID"
        });

        app.MapGet("/api/payments/provider-settings", async (int providerId, int custId, int method, int status, int transactionType, string languageCode, IPaymentsService service) =>
        {
            var settings = await service.GetProviderSettings(providerId, custId, method, status, transactionType, languageCode);
            return Results.Ok(settings);
        })
        .WithName("GetPaymentProviderSettings")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment provider settings",
            Description = "Returns the settings for a specific payment provider based on method, status, and transaction type"
        });

        app.MapPost("/api/payments/update-provider", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var provider = JsonConvert.DeserializeObject<UpdateProviderRequest>(body);
            var result = await service.UpdateProvider(provider);
            return Results.Ok(result);
        })
        .WithName("UpdateProvider")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update payment provider",
            Description = "Updates an existing payment provider's information"
        });

        app.MapGet("/api/payments/provider-fees/{providerId}", async (int providerId, string languageCode, IPaymentsService service) =>
        {
            var fees = await service.GetProviderFees(providerId, languageCode);
            return Results.Ok(fees);
        })
        .WithName("GetPaymentProviderFees")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment provider fees",
            Description = "Returns the fees for a specific payment provider"
        });

        app.MapGet("/api/payments/banks", async (string site, int providerId, int transactionType, string languageCode, IPaymentsService service) =>
        {
            var banks = await service.GetBanks(site, providerId, transactionType, languageCode);
            return Results.Ok(banks);
        })
        .WithName("GetAllBanks")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get all banks",
            Description = "Returns a list of all available banks"
        });

        MapManageProviderDailyWithdrawalCountEndpoints(app);
        MapManageProviderPaymentMethodEndpoints(app);
    }

    private static void MapManageProviderDailyWithdrawalCountEndpoints(WebApplication app) {
        app.MapGet("/api/payments/get-daily-withdrawal-count/{custId}", async (int custId, IPaymentsService service) =>
        {
            var count = await service.GetDailyWithdrawalCount(custId);
            return Results.Ok(count);
        })
        .WithName("GetDailyWithdrawalCount")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get daily withdrawal count",
            Description = "Returns the daily withdrawal count for a specific customer"
        });

        app.MapPost("/api/payments/set-daily-withdrawal-count", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var limitRequest = JsonConvert.DeserializeObject<SetDailyWithdrawalCountRequest>(body);
            
            if (limitRequest == null)
            {
                return Results.BadRequest("Invalid request data");
            }

            var result = await service.SetDailyWithdrawalCount(limitRequest);
            return Results.Ok(result);
        })
        .WithName("SetDailyWithdrawalCount")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Set daily withdrawal count",
            Description = "Sets the daily withdrawal count for a specific customer"
        });
    }

    private static void MapManageProviderPaymentMethodEndpoints(WebApplication app)
    {
        app.MapGet("/api/payments/get-add-payment-method-info", async (int custId, int providerId, int transactionType, string languageCode, IPaymentsService service) =>
        {
            var methods = await service.GetAddPaymentMethodInfo(custId, providerId, transactionType, languageCode);
            return Results.Ok(methods);
        })
        .WithName("GetAddPaymentMethodInfo")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get add payment method info",
            Description = "Returns a list of all payment methods and channel contact"
        });

        app.MapPost("/api/payments/add-payment-method", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var method = JsonConvert.DeserializeObject<InsertPaymentMethodRequest>(body);
            var result = await service.InsertPaymentMethod(method);
            return Results.Ok(result);
        })
        .WithName("AddPaymentMethod")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Add payment method",
            Description = "Adds a new payment method to the system"
        });

        app.MapPost("/api/payments/update-payment-method", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var method = JsonConvert.DeserializeObject<UpdatePaymentMethodRequest>(body);
            var result = await service.UpdatePaymentMethod(method);
            return Results.Ok(result);
        })
        .WithName("UpdatePaymentMethod")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update payment method",
            Description = "Updates an existing payment method"
        });
    }
} 