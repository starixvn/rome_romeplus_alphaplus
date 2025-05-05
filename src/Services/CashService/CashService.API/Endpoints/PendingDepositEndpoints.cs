using AlphaPlus.CashService.API.Models.PendingDepositWithdrawal;
using AlphaPlus.CashService.API.Services;
using Newtonsoft.Json;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PendingDepositEndpoints
{
    public static void MapPendingDepositEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Pending Deposit");

        group.MapGet("/member-deposit-login-names", async (IPendingDepositService service) =>
        {
            var members = await service.GetMemberDepositLoginNames();
            return Results.Ok(members);
        })
        .WithName("GetMemberDepositLoginNames")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member deposit login names",
            Description = "Returns a list of all available member login names for manual deposit"
        });

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

        group.MapPost("/add-deposit-account-info", async (HttpRequest request, IPendingDepositService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var accountInfo = JsonConvert.DeserializeObject<PendingInfoDetail>(body);

            if (accountInfo == null)
            {
                return Results.BadRequest("Invalid account information data");
            }

            var result = await service.InsertDepositAccountInfo(accountInfo);
            return Results.Ok(result);
        })
        .WithName("InsertDepositAccountInfo")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Insert deposit account information",
            Description = "Adds new account information for a manual deposit"
        });
    }
}