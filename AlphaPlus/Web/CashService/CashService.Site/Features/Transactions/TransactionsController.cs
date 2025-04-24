using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.CashService.Site.Features.Shared.Models;
using AlphaPlus.CashService.Site.Features.Transactions.Commands;
using AlphaPlus.CashService.Site.Features.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Transactions;

public class TransactionsController(
    IMediator mediator,
    IConfiguration configuration,
    HttpClient httpClient,
    ILogger<TransactionsController> logger
) : SiteBaseController(mediator)
{
    private readonly string _apiBaseUrl = configuration["CashServiceAPIUrl"] ?? throw new InvalidOperationException("CashServiceAPIUrl not configured.");

    public IActionResult Index() => Ok("Transactions");

    public IActionResult Deposit() => View();

    public IActionResult History() => View();

    public async Task<IActionResult> GetAll(string transactionType,
        string status, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 1000)
    {
        var query = new GetTransactionsQuery
        {
            TransactionType = transactionType,
            Status = status,
            FromDate = fromDate,
            ToDate = toDate,
            Page = page,
            PageSize = pageSize
        };

        var result = await mediator.Send(query);
        return Json(result);
    }

    public async Task<IActionResult> Detail(int transactionId)
    {
        try
        {
            var query = new GetTransactionByIdQuery(transactionId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Transaction not found");
            return NotFound(new { error = "Transaction not found" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting transaction by ID");
            return StatusCode(500, new { error = "Failed to fetch transaction" });
        }
    }

    public async Task<IActionResult> GetMemberBankAccounts()
    {
        var url = $"{_apiBaseUrl}/member-bank-accounts";
        var memberAccounts = await httpClient.GetFromJsonAsync<object>(url);
        return Json(memberAccounts);
    }

    [HttpPost("RevertTransaction")]
    public async Task<IActionResult> RevertTransactionToPending([FromBody] RevertTransactionRequest request)
    {
        if (request.TransactionId <= 0)
        {
            return BadRequest(new ResultModel { Code = StatusCodes.Status400BadRequest, Message = "Invalid TransactionId." });
        }

        try
        {
            var command = new RevertTransactionCommand(request.TransactionId);
            var result = await mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error processing RevertTransactionCommand for {TransactionId}", request.TransactionId);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResultModel { Code = StatusCodes.Status500InternalServerError, Message = "An unexpected internal error occurred." });
        }
    }
}
