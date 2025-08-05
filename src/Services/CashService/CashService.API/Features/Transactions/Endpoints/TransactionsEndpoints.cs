namespace AlphaPlus.CashService.API.Features.Transactions.Endpoints;

using AlphaPlus.CashService.API.Features.Transactions.Models;
using Criteria;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Services;

public static class TransactionsEndpoints
{
    public static void MapTransactionsHistoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/transactions")
            .WithTags("Transactions History");

        group.MapGet("/history", async ([FromQuery] DateTime? fromDate
                , [FromQuery] DateTime? toDate
                , [FromQuery] int custId
                , [FromQuery] int paymentStatus
                , [FromQuery] int transactionType
                , [FromQuery] int siteDbGroupId
                , [FromQuery] string account
                , ITransactionService service) =>
            {
                var criteria = new GetAllTransactionsWithPagingCriteria
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    CustId = custId,
                    PaymentStatus = paymentStatus,
                    TransactionType = transactionType,
                    SiteDbGroupId = siteDbGroupId,
                    PaymentTo = transactionType == 1 ? account : string.Empty,
                    PaymentFrom = transactionType == 1 ? string.Empty : account
                };
                var transactionHistory = await service.GetTransactionsAsync(criteria);
                return Results.Ok(transactionHistory);
            })
        .WithName("GetTransactionHistory")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Get transaction history by filter",
            Description = "Returns a list of transaction base on filter"
        });

        group.MapGet("/detail", async ([FromQuery] long transactionId
                , [FromQuery] int custId
                , [FromQuery] short siteDbGroupId
                , ITransactionService service) =>
        {
            var transactionDetail = await service.GetTransactionByIdAsync(transactionId, custId, siteDbGroupId);
            return Results.Ok(transactionDetail);
        }).WithName("GetTransactionDetail")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Get Transaction details",
            Description = "Returns transaction details"
        });

        group.MapPost("/revert-pending-deposit", async (ITransactionService service, [FromBody] RevertTransaction model) =>
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

        group.MapPost("/revert-pending-withdrawal", async (ITransactionService service, [FromBody] RevertTransaction model) =>
        {
            var result = await service.RevertWithdrawalTransactionToPending(model);
            return Results.Ok(result);
        })
        .WithName("RevertWithdrawalTransactionToPending")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Revert transaction to pending",
            Description = "Revert transaction to pending"
        });

        group.MapGet("/get-members", async ([FromQuery] int agentId, [FromQuery] short custDbGroupId, ITransactionService service) =>
        {
            var members = await service.GetMembersAsync(agentId, custDbGroupId);
            return Results.Ok(members);
        })
        .WithName("GetMembersAsync")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Get Cash Member List of Agent",
            Description = "Returns Cash Member List of Agent"
        });

        group.MapGet("/agent-bank-list", async ([FromQuery] int agentId
                , [FromQuery] int transactionType
                , [FromQuery] int custDbGroupId
                , IBankService service) =>
            {
                var bankList = await service.GetCashAgentBankInfoAsync(agentId, transactionType, custDbGroupId);
                return Results.Ok(bankList);
            })
            .WithName("GetCashAgentBankList")
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get CashAgent Bank List",
                Description = "Returns a list of bank"
            });

        group.MapGet("/member-bank-list", async ([FromQuery] int custId, [FromQuery] int custDbGroupId, IBankService service) =>
            {
                var bankList = await service.GetMemberBankInfoAsync(custId, custDbGroupId);
                return Results.Ok(bankList);
            })
            .WithName("GetCashMemberBankList")
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get Cash Member Bank List",
                Description = "Returns a list of bank"
            });

        group.MapGet("/payment-provider-list", async ([FromQuery] int agentId
                , [FromQuery] int transactionType
                , [FromQuery] int custDbGroupId
                , IBankService service) =>
            {
                var paymentProviderList = await service.GetCashAgentProviderAsync(agentId, transactionType, custDbGroupId);
                return Results.Ok(paymentProviderList);
            })
            .WithName("GetCashAgentProviderAsync")
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get Cash Agent Provider List",
                Description = "Returns a list of bank"
            });
    }
}