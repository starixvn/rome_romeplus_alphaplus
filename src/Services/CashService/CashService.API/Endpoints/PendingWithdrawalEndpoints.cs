using AlphaPlus.CashService.API.Models.PendingDepositWithdrawal;
using AlphaPlus.CashService.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PendingWithdrawalEndpoints
{
    public static void MapPendingWithdrawalEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Pending Withdrawal");

        group.MapGet("/member-login-names", async (IPendingWithdrawalService service) =>
        {
            var members = await service.GetMemberLoginName();
            return Results.Ok(members);
        })
        .WithName("GetMemberLoginNames")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member login names",
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

        group.MapPost("/add-account-info", async (HttpRequest request, IPendingWithdrawalService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var accountInfo = JsonConvert.DeserializeObject<PendingInfoDetail>(body);
            
            if (accountInfo == null)
            {
                return Results.BadRequest("Invalid account information data");
            }

            var result = await service.InsetAccountInfo(accountInfo);
            return Results.Ok(result);
        })
        .WithName("InsertAccountInfo")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Insert account information",
            Description = "Adds new account information for a manual withdrawal"
        });
    }
}
