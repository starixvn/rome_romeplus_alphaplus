using AlphaPlus.CashService.API.Enum;
using AlphaPlus.CashService.API.Models.PendingCommons;
using AlphaPlus.CashService.API.Models.Transactions;
using AlphaPlus.CashService.API.Services;
using Newtonsoft.Json;

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

        group.MapGet("/limit-amount", async ([FromQuery] int agentId
            , [FromQuery] int custId
            , [FromQuery] int custDbGroupId
            , IPendingDepositService service) =>
        {
            var creditAmt = await service.GetLimitAmtDepositAsync(agentId, custId, custDbGroupId);
            return Results.Ok(creditAmt);
        })
        .WithName("GetLimitAmtDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get limit amount deposit information",
            Description = "Returns range value the member can deposit (MinAmt - MaxAmt)"
        });

        group.MapGet("/member-setting", async ([FromQuery] int custId,
            [FromQuery] int custDbGroupId
            , IPendingDepositService service) =>
        {
            var memberSettingDeposit = await service.GetMemberSettingDepositAsync(custId, custDbGroupId);
            return Results.Ok(memberSettingDeposit);
        })
        .WithName("GetMemberSettingDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member setting deposit information",
            Description = "Returns deposit setting value for Member - use to validate min-max amount"
        });

        group.MapPost("/create", async ([FromBody] CreateManualModel param, IPendingDepositService service) =>
        {
            var result = await service.AddManualDepositAsync(param);
            return Results.Ok(result);
        })
        .WithName("CreateManualDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Create manual deposit",
            Description = "Adding manual deposit transaction on Agency site"
        });

        group.MapPost("/approve", async ([FromBody] ProcessTransactionModel model, IPendingDepositService service) =>
        {
            model.StatusId = (int)TransactionStatus.Completed;
            var result = await service.ProcessDepositAsync(model);
            return Results.Ok(result);
        })
        .WithName("ProcessApproveDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Approve deposit (completed)",
            Description = "Approve deposit transaction"
        });

        group.MapPost("/reject", async ([FromBody] ProcessTransactionModel model, IPendingDepositService service) =>
        {
            model.StatusId = (int)TransactionStatus.Rejected;
            var result = await service.ProcessDepositAsync(model);
            return Results.Ok(result);
        })
        .WithName("ProcessRejectDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Reject deposit",
            Description = "Reject deposit transaction"
        });
    }
}