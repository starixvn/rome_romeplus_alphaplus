using AlphaPlus.CashService.API.Models.PendingCommons;
using AlphaPlus.CashService.API.Services;
using Newtonsoft.Json;

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

    }
}