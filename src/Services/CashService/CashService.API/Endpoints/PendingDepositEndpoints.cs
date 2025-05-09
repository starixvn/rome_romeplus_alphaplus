using AlphaPlus.CashService.API.Models.PendingCommons;
using AlphaPlus.CashService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PendingDepositEndpoints
{
    public static void MapPendingDepositEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/deposit")
            .WithTags("Pending Deposit");

        group.MapGet("/member-bank-accounts", async (IPendingDepositService service) =>
        {
            var bankAccounts = await service.GetMemberBankAccountInfo();
            return Results.Ok(bankAccounts);
        })
        .WithName("GetMemberBankAccounts")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member bank accounts",
            Description = "Returns a list of bank accounts for members"
        });

        group.MapGet("/accumulate-credit/{custId}", async (int custId, IPendingDepositService service) =>
        {
            var accumulateCredit = await service.GetAccumulateCreditAsync(custId);
            return Results.Ok(accumulateCredit);
        })
        .WithName("GetAccumulateCreditAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get accumulate credit deposit information",
            Description = "Returns total Cash member return Credit of Agent"
        });

        group.MapGet("/cash-deposit-limit-amount/{custId}/{accumCreditReturn}", async (int custId, decimal accumCreditReturn, IPendingDepositService service) =>
        {
            var creditAmt = await service.GetLimitAmtDepositAsync(custId, accumCreditReturn);
            return Results.Ok(creditAmt);
        })
        .WithName("GetLimitAmtDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get limit amount deposit information",
            Description = "Returns range value the member can deposit (MinAmt - MaxAmt)"
        });

        group.MapGet("/member-setting-deposit/{custId}", async (int custId, IPendingDepositService service) =>
        {
            var memberSettingDeposit = await service.GetMemberSettingDepositAsync(custId);
            return Results.Ok(memberSettingDeposit);
        })
        .WithName("GetMemberSettingDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member setting deposit information",
            Description = "Returns range value the member can deposit (MinAmt - MaxAmt)"
        });

        group.MapPost("/create-manual-deposit", async ([FromBody] CreateManualModel param, IPendingDepositService service) =>
        {
            var (result, id, message) = await service.CreateManualDepositAsync(param);
            return result.Code != 0 ? Results.NotFound() : Results.Ok(new { result, id, message });
        })
        .WithName("CreateManualDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Create manual deposit",
            Description = "Adding manual deposit transaction on Agency site"
        });
    }
}