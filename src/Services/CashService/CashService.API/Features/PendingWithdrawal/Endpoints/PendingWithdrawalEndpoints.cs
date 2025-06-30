namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Endpoints;

using AlphaPlus.CashService.API.Features.Transactions.Enum;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

public static class PendingWithdrawalEndpoints
{
    public static void MapPendingWithdrawalEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/withdrawal")
            .WithTags("Pending Withdrawal");

        group.MapGet("/limit-amount", async ([FromQuery] int custId,
                [FromQuery] int uplineId,
                [FromQuery] int providerId,
                [FromQuery] int paymentMethodId,
                [FromQuery] int custDbGroupId,
                IPendingWithdrawalService service) =>
        {
            var cashLimit = await service.GetLimitAmtWithdrawAsync(new GetCashLimitAmt
            {
                CustId = custId,
                UplineId = uplineId,
                ProviderId = providerId,
                CustDbGroupId = custDbGroupId,
                PaymentMethodId = paymentMethodId
            });
            return Results.Ok(cashLimit);
        })
        .WithName("GetLimitAmtWithdrawAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get limit amount withdrawal information",
            Description = "Returns range value the member can withdrawal (MinAmt - MaxAmt)"
        });

        group.MapGet("/member-cash-balance",
            async (int custId, int siteDbGroupId, IPendingWithdrawalService service) =>
            {
                var cashBalance = await service.GetCashBalanceAsync(custId, siteDbGroupId);
                return Results.Ok(cashBalance);
            });

        group.MapGet("/member-setting/{custId}", async (int custId, IPendingWithdrawalService service) =>
        {
            var memberSettingWithdraw = await service.GetMemberSettingWithdrawAsync(custId);
            return Results.Ok(memberSettingWithdraw);
        })
        .WithName("GetMemberSettingWithdrawAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member setting withdrawal information",
            Description = "Returns withdrawal setting value for Member - use to validate min-max amount"
        });

        group.MapPost("/create", async ([FromBody] AddManualWithdrawalModel model, IPendingWithdrawalService service) =>
        {
            var result = await service.AddManualWithdrawAsync(model);
            return Results.Ok(result);
        })
        .WithName("CreateManualWithdrawAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Create manual withdrawal",
            Description = "Adding manual withdrawal transaction on Agency site"
        });

        group.MapPost("/approve", async ([FromBody] ProcessWithdrawalTransaction model, IPendingWithdrawalService service) =>
            {
                model.StatusId = (int)TransactionStatus.Completed;
                var result = await service.ProcessWithdrawAsync(model);
                return Results.Ok(result);
            })
            .WithName("ApproveTransactionAsync")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Approve withdrawal (completed)",
                Description = "Approve withdrawal"
            });

        group.MapPost("/reject", async ([FromBody] ProcessWithdrawalTransaction model, IPendingWithdrawalService service) =>
            {
                model.StatusId = (int)TransactionStatus.Rejected;
                model.PaymentFrom = string.Empty;
                model.PaymentMethodId = 0;
                model.ProviderId = 0;
                var result = await service.ProcessWithdrawAsync(model);
                return Results.Ok(result);
            })
            .WithName("RejectTransactionAsync")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Reject withdrawal",
                Description = "Reject withdrawal"
            });
    }
}