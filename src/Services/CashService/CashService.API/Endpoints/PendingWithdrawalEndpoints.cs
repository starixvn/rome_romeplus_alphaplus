using AlphaPlus.CashService.API.Models.PendingCommons;
using AlphaPlus.CashService.API.Services;
using Newtonsoft.Json;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PendingWithdrawalEndpoints
{
    public static void MapPendingWithdrawalEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Pending Withdrawal");

        group.MapGet("/member-withdraw-login-names", async (IPendingWithdrawalService service) =>
        {
            var members = await service.GetMemberWithdrawalLoginNames();
            return Results.Ok(members);
        })
        .WithName("GetMemberWithdrawLoginNames")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member withdrawal login names",
            Description = "Returns a list of all available member login names for manual withdrawal"
        });

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

        group.MapPost("/add-withdraw-account-info", async (HttpRequest request, IPendingWithdrawalService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var accountInfo = JsonConvert.DeserializeObject<PendingInfoDetail>(body);

            if (accountInfo == null)
            {
                return Results.BadRequest("Invalid account information data");
            }

            var result = await service.InsertWithdrawAccountInfo(accountInfo);
            return Results.Ok(result);
        })
        .WithName("InsertWithdrawAccountInfo")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Insert withdrawal account information",
            Description = "Adds new account information for a manual withdrawal"
        });
    }
}
