using AlphaPlus.CashService.API.Features.PendingDeposit.Models;
using AlphaPlus.CashService.API.Features.PendingDeposit.Services;
using AlphaPlus.CashService.API.Features.Transactions.Enum;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.API.Features.PendingDeposit.Endpoints;

using Microsoft.OpenApi.Models;
using Transactions.Models;
using Transactions.Services;

public static class PendingDepositEndpoints
{
    public static void MapPendingDepositEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/deposit")
            .WithTags("Pending Deposit");

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
            , ITransactionService service) =>
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

        group.MapPost("/create", async ([FromBody] CreateManualDepositModel param, IPendingDepositService service) =>
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

        group.MapPost("/revert-pending", async (IPendingDepositService service, [FromBody] RevertTransaction model) =>
            {
                var result = await service.RevertDepositTransactionToPending(model);
                return Results.Ok(result);
            })
            .WithName("RevertDepositTransactionToPending")
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Revert transaction to pending",
                Description = "Revert transaction to pending"
            });

        group.MapPost("/approve", async ([FromBody] ProcessDepositTransaction model, IPendingDepositService service) =>
        {
            var memberSetting = await service.GetMemberSettingDepositWhenProcessTransactionAsync(model.CustId, model.ActorId, model.CustDbGroupId);
            model.AgentGivenCredit = memberSetting.AgentGivenCredit;
            model.CreditMembersGivenCredit = memberSetting.CreditMembers_GivenCredit;
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

        group.MapPost("/reject", async ([FromBody] ProcessDepositTransaction model, IPendingDepositService service) =>
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