using AlphaPlus.CashService.API.Models.PendingCommons;
using AlphaPlus.CashService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PendingWithdrawalEndpoints
{
    public static void MapPendingWithdrawalEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/withdrawal")
            .WithTags("Pending Withdrawal");

        group.MapGet("/bank-withdrawal-accounts", async (IPendingWithdrawalService service) =>
        {
            var bankAccounts = await service.GetBankWithdrawalAccounts();
            return Results.Ok(bankAccounts);
        })
        .WithName("GetBankWithdrawalAccounts")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get bank withdrawal accounts",
            Description = "Returns a list of bank withdrawal accounts"
        });

        group.MapGet("/cash-balance/{custId}", async (int custId, IPendingWithdrawalService service) =>
        {
            var cashBalance = await service.GetCashBalanceAsync(custId);
            return Results.Ok(cashBalance);
        })
        .WithName("GetCashBalanceAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get cash balance of member",
            Description = "Returns cash balance of member"
        });

        group.MapGet("/limit-amount", async ([FromQuery] int custId,
                [FromQuery] int uplineId,
                [FromQuery] int providerId,
                [FromQuery] int paymentMethodId,
                IPendingWithdrawalService service) =>
        {
            var cashLimit = await service.GetLimitAmtWithdrawAsync(new CashLimitAmt
            {
                CustId = custId,
                UplineId = uplineId,
                ProviderId = providerId,
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
    }
}