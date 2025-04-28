using AlphaPlus.CashService.API.Models.PendingDepositWithdrawal;
using AlphaPlus.CashService.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PendingDepositEndpoints
{
    public static void MapPendingDepositEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Pending Deposit");

        group.MapGet("/member-login-names", async (IPendingDepositService service) =>
        {
            var members = await service.GetMemberLoginName();
            return Results.Ok(members);
        })
        .WithName("GetMemberLoginNames")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member login names",
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

        group.MapPost("/add-account-info", async (HttpRequest request, IPendingDepositService service) =>
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
            Description = "Adds new account information for a manual deposit"
        });
    }
} 