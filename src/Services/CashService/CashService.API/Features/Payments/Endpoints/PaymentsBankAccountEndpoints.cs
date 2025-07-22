using AlphaPlus.CashService.API.Features.Payments.Requests;
using Newtonsoft.Json;
using AlphaPlus.CashService.API.Features.Payments.Services;

namespace AlphaPlus.CashService.API.Features.Payments.Endpoints;

public static class PaymentsBankAccountEndpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapGet("/api/payments/banks/{custId}", async (int custId, string languageCode, IPaymentsService service) =>
        {
            var banks = await service.GetBanksByCust(custId, languageCode);
            return Results.Ok(banks);
        })
        .WithName("GetBanksByCust")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get banks by customer",
            Description = "Returns a list of banks associated with a specific customer"
        });
        
        app.MapGet("/api/payments/transaction-types", async (IPaymentsService service) =>
        {
            var transactionTypes = await service.GetTransactionTypes();
            return Results.Ok(transactionTypes);
        })
        .WithName("GetTransactionTypes")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get transaction types",
            Description = "Returns a list of transaction types for a specific bank"
        });

        MapManageBankAccountEndpoints(app);
        MapManageTransactionLimitEndpoints(app);
    }

    private static void MapManageTransactionLimitEndpoints(WebApplication app)
    {
        app.MapGet("/api/payments/transaction-limit/{custId}", async (int custId, IPaymentsService service) =>
        {
            var limit = await service.GetTransactionLimit(custId);
            return Results.Ok(limit);
        })
        .WithName("GetTransactionLimit")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get transaction limit",
            Description = "Returns the transaction limits for a specific customer"
        });

        app.MapPost("/api/payments/set-transaction-limit", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var limit = JsonConvert.DeserializeObject<SetTransactionLimitRequest>(body);
            var result = await service.SetTransactionLimit(limit);
            return Results.Ok(result);
        })
        .WithName("SetTransactionLimit")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Set transaction limit",
            Description = "Sets transaction limits for a specific customer"
        });
    }

    private static void MapManageBankAccountEndpoints(WebApplication app)
    { 
        app.MapGet("/api/payments/bank-accounts", async (
            int custId, 
            int bankId,
            int transactionType,
            int status,
            string searchText,
            string languageCode,
            IPaymentsService service) =>
        {
            var bankAccounts = await service.GetBankAccounts(custId, bankId, transactionType, status, searchText, languageCode);
            return Results.Ok(bankAccounts);
        })
        .WithName("GetBankAccounts")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get bank accounts",
            Description = "Returns a list of bank accounts for a specific customer and transaction type with optional filtering by bank, account number, account holder name, and status"
        });

        app.MapPost("/api/payments/add-bank-account", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var bankAccount = JsonConvert.DeserializeObject<AddBankAccountRequest>(body);
            var result = await service.AddBankAccount(bankAccount);
            return Results.Ok(result);
        })
        .WithName("AddBankAccount")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Add bank account",
            Description = "Adds a new bank account for a customer"
        });

        app.MapGet("/api/payments/bank-accounts/{accountId}", async (int accountId, string languageCode, IPaymentsService service) =>
        {
            var bankAccount = await service.GetBankAccountById(accountId, languageCode);
            return Results.Ok(bankAccount);
        })
        .WithName("GetBankAccountById")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get bank account by ID",
            Description = "Returns a specific bank account by its ID"
        });

        app.MapPost("/api/payments/update-bank-account", async (HttpRequest request, IPaymentsService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var bankAccount = JsonConvert.DeserializeObject<UpdateBankAccountRequest>(body);
            var result = await service.UpdateBankAccount(bankAccount);
            return Results.Ok(result);
        })
        .WithName("UpdateBankAccount")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update bank account",
            Description = "Updates an existing bank account's information"
        });

        app.MapPost("/api/payments/bank-accounts/delete", async (HttpRequest request,IPaymentsService service) =>
        { 
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var deleteBankAccountRequest = JsonConvert.DeserializeObject<DeleteBankAccountRequest>(body);
            var result = await service.DeleteBankAccount(deleteBankAccountRequest);
            return Results.Ok(result);
        })
        .WithName("DeleteBankAccount")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Delete bank account",
            Description = "Deletes a specific bank account by its ID"
        });
    }
} 