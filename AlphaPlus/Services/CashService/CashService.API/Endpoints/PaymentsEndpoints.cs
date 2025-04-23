using AlphaPlus.CashService.API.Services;
using AlphaPlus.CashService.API.Models.Payments;
using AlphaPlus.CashService.API.Requests;
using Newtonsoft.Json;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/payments/providers-by-custid/{custId}", async (int custId, IPaymentsService service) =>
        {
            var providers = await service.GetProviders(custId);
            return Results.Ok(providers);
        })
        .WithName("GetPaymentProviders")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment providers",
            Description = "Returns a list of available payment providers"
        });

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

            var result = await service.SetDailyWithdrawalCount(limitRequest.CustId, limitRequest.Count);
            return Results.Ok(result);
        })
        .WithName("SetDailyWithdrawalCount")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Set daily withdrawal count",
            Description = "Sets the daily withdrawal count for a specific customer"
        });

        app.MapGet("/api/payments/provider-fees/{custId}", async (int custId, IPaymentsService service) =>
        {
            var fees = await service.GetProviderFees(custId);
            return Results.Ok(fees);
        })
        .WithName("GetPaymentProviderFees")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment provider fees",
            Description = "Returns the fees for a specific payment provider"
        });

        app.MapGet("/api/payments/providers/{custId}/{providerId}", async (int custId, int providerId, IPaymentsService service) =>
        {
            var provider = await service.GetProviderById(custId, providerId);
            return Results.Ok(provider);
        })
        .WithName("GetPaymentProviderById")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment provider by ID",
            Description = "Returns a specific payment provider by its ID"
        });

        app.MapGet("/api/payments/provider-settings", async (int providerId, int method, int status, int transactionType, IPaymentsService service) =>
        {
            var settings = await service.GetProviderSettings(providerId, method, status, transactionType);
            return Results.Ok(settings);
        })
        .WithName("GetPaymentProviderSettings")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment provider settings",
            Description = "Returns the settings for a specific payment provider based on method, status, and transaction type"
        });

        app.MapGet("/api/payments/payment-methods", async (IPaymentsService service) =>
        {
            var methods = await service.GetPaymentMethods();
            return Results.Ok(methods);
        })
        .WithName("GetPaymentMethods")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get payment methods",
            Description = "Returns a list of all payment methods"
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

        app.MapPost("/api/payments/add-payment-method", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var method = JsonConvert.DeserializeObject<PaymentMethodModel>(body);
            var result = await service.InsertPaymentMethod(method);
            return Results.Ok(result);
        })
        .WithName("AddPaymentMethod")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Add payment method",
            Description = "Adds a new payment method to the system"
        });
        
        app.MapGet("/api/payments/banks", async (IPaymentsService service) =>
        {
            var banks = await service.GetAllBanks();
            return Results.Ok(banks);
        })
        .WithName("GetAllBanks")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get all banks",
            Description = "Returns a list of all available banks"
        });


        


        app.MapGet("/api/bank-accounts", async (
            int custId, 
            IPaymentsService service,
            int bankId,
            int transactionType,
            int status,
            string searchText) =>
        {
            var bankAccounts = await service.GetBankAccounts(custId, bankId, transactionType, status, searchText);
            return Results.Ok(bankAccounts);
        })
        .WithName("GetBankAccounts")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get bank accounts",
            Description = "Returns a list of bank accounts for a specific customer and transaction type with optional filtering by bank, account number, account holder name, and status"
        });

        app.MapPost("/api/add-bank-account", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var bankAccount = JsonConvert.DeserializeObject<BankAccountModel>(body);
            
            if (bankAccount == null)
            {
                return Results.BadRequest("Invalid bank account data");
            }

            var result = await service.AddBankAccount(bankAccount);
            return Results.Ok(result);
        })
        .WithName("AddBankAccount")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Add bank account",
            Description = "Adds a new bank account for a customer"
        });

        app.MapDelete("/api/bank-accounts/{accountId}", async (int accountId, IPaymentsService service) =>
        {
            var result = await service.DeleteBankAccount(accountId);
            return Results.Ok(result);
        })
        .WithName("DeleteBankAccount")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Delete bank account",
            Description = "Deletes a specific bank account by its ID"
        });

        app.MapPost("/api/set-transaction-limit", async (int custId, HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var limit = JsonConvert.DeserializeObject<TransactionLimitModel>(body);
            
            if (limit == null)
            {
                return Results.BadRequest("Invalid transaction limit data");
            }

            var result = await service.SetTransactionLimit(custId, limit);
            return Results.Ok(result);
        })
        .WithName("SetTransactionLimit")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Set transaction limit",
            Description = "Sets transaction limits for a specific customer"
        });

        app.MapPut("/api/update-bank-account", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var bankAccount = JsonConvert.DeserializeObject<BankAccountModel>(body);
            
            if (bankAccount == null)
            {
                return Results.BadRequest("Invalid bank account data");
            }

            var result = await service.UpdateBankAccount(bankAccount);
            return Results.Ok(result);
        })
        .WithName("UpdateBankAccount")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update bank account",
            Description = "Updates an existing bank account's information"
        });
    }
} 