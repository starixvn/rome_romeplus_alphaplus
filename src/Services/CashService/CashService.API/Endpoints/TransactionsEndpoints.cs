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

        group.MapGet("/get-members", async ([FromQuery] int agentId, [FromQuery] string siteDbIp, ITransactionService service) =>
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

        group.MapGet("/agent-bank-list", async ([FromQuery] int agentId
                , [FromQuery] int transactionType
                , [FromQuery] string siteDbIp
                , IBankService service) =>
            {
                var bankList = await service.GetCashAgentBankInfoAsync(agentId, transactionType, siteDbIp);
                return Results.Ok(bankList);
            })
            .WithName("GetCashAgentBankList")
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get CashAgent Bank List",
                Description = "Returns a list of bank"
            });

        group.MapGet("/member-bank-list", async ([FromQuery] int custId, [FromQuery] string siteDbIp, IBankService service) =>
            {
                var bankList = await service.GetMemberBankInfoAsync(custId, siteDbIp);
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
                , [FromQuery] string siteDbIp
                , IBankService service) =>
            {
                var bankList = await service.GetCashAgentProviderAsync(agentId, transactionType, siteDbIp);
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