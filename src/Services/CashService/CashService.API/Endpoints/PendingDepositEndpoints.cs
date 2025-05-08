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

        group.MapGet("/accumulate-credit/{id}", async (int id, IPendingDepositService service) =>
        {
            var accumulateCredit = await service.GetAccumulateCreditAsync(id);
            return Results.Ok(accumulateCredit);
        })
        .WithName("GetAccumulateCreditAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get accumulate credit deposit information",
            Description = "Returns total Cash member return Credit of Agent"
        });

        group.MapGet("/member-setting-deposit/{id}", async (int id, IPendingDepositService service) =>
        {
            var memberSettingDeposit = await service.GetMemberSettingDepositAsync(id);
            return Results.Ok(memberSettingDeposit);
        })
        .WithName("GetMemberSettingDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member setting deposit information",
            Description = "Returns range value the member can deposit (MinAmt - MaxAmt)"
        });

        group.MapPost("/create-manual-deposit", async ([FromBody] CreateManualDeposit param, IPendingDepositService service) =>
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