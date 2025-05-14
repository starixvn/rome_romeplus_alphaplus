namespace AlphaPlus.CashService.API.Endpoints;

using Criteria.Transactions;
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
                , [FromQuery] int? account
                , [FromQuery] string status
                , [FromQuery] int transactionType
                , [FromQuery] int page
                , [FromQuery] int pageSize
                , ITransactionService service) =>
            {
                var criteria = new GetAllTransactionsWithPagingCriteria()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    Account = account,
                    Status = status,
                    TransactionType = transactionType,
                    Page = page,
                    PageSize = pageSize
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

        group.MapGet("/details/{transactionId:int}", async (int transactionId, ITransactionService service) =>
        {
            var transactionDetail = await service.GetTransactionByIdAsync(transactionId);
            return Results.Ok(transactionDetail);
        })
        .WithName("GetTransactionDetail")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Get Transaction details",
            Description = "Returns transaction details"
        });

        group.MapPost("/revert-pending", async (ITransactionService service, [FromBody] UpdateTransactionStatusRemarkCommand command) =>
        {
            command.Status = "Pending";
            var transaction = await service.UpdateTransactionStatus(command);
            if (transaction == null)
                return Results.NotFound();

            return Results.Ok(transaction);
        })
        .WithName("RevertTransactionToPending")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Revert transaction to pending",
            Description = "Revert transaction to pending"
        });

        group.MapGet("/get-members/{agentId}/{siteDbIp}", async (int agentId, string siteDbIp, ITransactionService service) =>
        {
            var members = await service.GetMembersAsync(agentId, siteDbIp);
            return Results.Ok(members);
        })
        .WithName("GetMembersAsync")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Get Cash Member List of Agent",
            Description = "Returns Cash Member List of Agent"
        });

        group.MapPut("/sync-credit/{cashAccumCreditTbl}", async (string cashAccumCreditTbl, ITransactionService service) =>
        {
            var syncCash = await service.SyncCashAccumCreditAsync(cashAccumCreditTbl);
            return syncCash.Code != 0 ? Results.NotFound() : Results.Ok(new { syncCash });
        })
        .WithName("SyncCashAccumCreditAsync")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Sync Cash Accum Credit",
            Description = "Manually sync data CashAccumCredit to DBCUST.custdb"
        });
    }
}