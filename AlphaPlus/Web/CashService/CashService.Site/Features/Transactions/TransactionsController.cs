using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.CashService.Site.Features.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Transactions;

public class TransactionsController(IMediator mediator, ILogger<TransactionsController> logger)
    : SiteBaseController(mediator)
{
    public IActionResult Index() => Ok("Transactions");

    public IActionResult Deposit() => View();

    public IActionResult History() => View();

    [HttpGet]
    public async Task<IActionResult> GetTransactionsAsync(
        [FromQuery] string username,
        [FromQuery] string referenceNumber,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 1000,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTransactionsQuery
            {
                Username = username,
                ReferenceNumber = referenceNumber,
                FromDate = fromDate,
                ToDate = toDate,
                Page = page,
                PageSize = pageSize
            };

            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting transactions");
            return StatusCode(500, new { error = "Failed to fetch transactions" });
        }
    }

    [HttpGet("{transactionId}")]
    public async Task<IActionResult> GetTransactionByIdAsync(
        int transactionId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTransactionByIdQuery(transactionId);
            var result = await mediator.Send(query, cancellationToken);
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
}