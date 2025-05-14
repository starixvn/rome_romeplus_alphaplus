using AlphaPlus.CashService.API.Models.PendingCommons;
using AlphaPlus.CashService.API.Services;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
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
            , [FromQuery] string siteDbIp
            , IPendingDepositService service) =>
        {
            var creditAmt = await service.GetLimitAmtDepositAsync(agentId, custId, siteDbIp);
            return Results.Ok(creditAmt);
        })
        .WithName("GetLimitAmtDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get limit amount deposit information",
            Description = "Returns range value the member can deposit (MinAmt - MaxAmt)"
        });

        group.MapGet("/member-setting/{custId}", async (int custId, IPendingDepositService service) =>
        {
            var memberSettingDeposit = await service.GetMemberSettingDepositAsync(custId);
            return Results.Ok(memberSettingDeposit);
        })
        .WithName("GetMemberSettingDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member setting deposit information",
            Description = "Returns deposit setting value for Member - use to validate min-max amount"
        });

        group.MapPost("/create", async (HttpRequest request, IPendingDepositService service) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var provider = JsonConvert.DeserializeObject<CreateManualModel>(body);

            var result = await service.AddManualDepositAsync(provider, provider.SiteDbIp);
            return Results.Ok(result);
        })
        .WithName("CreateManualDepositAsync")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Create manual deposit",
            Description = "Adding manual deposit transaction on Agency site"
        });
    }
}