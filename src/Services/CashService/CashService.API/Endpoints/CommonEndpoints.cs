namespace AlphaPlus.CashService.API.Endpoints;

using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Services;

public static class CommonEndpoints
{
    public static void MapCommonEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/common")
            .WithTags("Common Get APIs");

        group.MapGet("country-codes/{siteName}", async (string siteName, ICommonService service) =>
            {
                var prefixes = await service.GetCountryCodes(siteName);
                return Results.Ok(prefixes);
            })
            .WithName("GetCountryCodes")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get country codes",
                Description = "Returns a list of all available country codes"
            });

        group.MapGet("/agent-bank-list", async ([FromQuery] int agentId
            , [FromQuery] int transactionType
            , IBankService service) =>
        {
            var bankList = await service.GetCashAgentBankInfoAsync(agentId, transactionType);
            return Results.Ok(bankList);
        })
        .WithName("GetCashAgentBankList")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Get CashAgent Bank List",
            Description = "Returns a list of bank"
        });

        group.MapGet("/member-bank-list", async ([FromQuery] int custId
            , IBankService service) =>
        {
            var bankList = await service.GetMemberBankInfoAsync(custId);
            return Results.Ok(bankList);
        })
        .WithName("GetCashMemberBankList")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Get Cash Member Bank List",
            Description = "Returns a list of bank"
        });

        group.MapGet("/provider-bank-list", async ([FromQuery] int agentId
            , [FromQuery] int transactionType
            , IBankService service) =>
        {
            var bankList = await service.GetCashAgentProviderAsync(agentId, transactionType);
            return Results.Ok(bankList);
        })
        .WithName("GetCashAgentProviderAsync")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Get Cash Agent Provider List",
            Description = "Returns a list of bank"
        });
    }
}