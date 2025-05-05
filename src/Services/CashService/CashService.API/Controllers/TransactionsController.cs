namespace AlphaPlus.CashService.API.Controllers;

using System;
using System.Threading.Tasks;
using Criteria.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Transactions;
using Services;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
    : ControllerBase
{
    [HttpGet("history")]
    public async Task<ActionResult<TransactionResponse>> GetTransactions([FromQuery] GetAllTransactionsWithPagingCriteria criteria)
    {
        var result = await transactionService.GetTransactionsAsync(criteria);
        return Ok(result);
    }

    [HttpGet("details")]
    public async Task<ActionResult<TransactionModel>> GetTransactionById(int transactionId)
    {
        var transaction = await transactionService.GetTransactionByIdAsync(transactionId);
        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpPost("revert-pending")]
    public async Task<ActionResult<TransactionModel>> UpdateStatus([FromBody] UpdateTransactionStatusRemarkCommand command)
    {
        command.Status = "Pending";
        var transaction = await transactionService.UpdateTransactionStatus(command);
        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpGet("get-account")]
    public async Task<IActionResult> GetAccount(CancellationToken cancellationToken)
    {
        logger.LogInformation("API endpoint GetAccount called");
        try
        {
            var criteria = new GetAccountCriteria();

            var accounts = await transactionService.GetAccountAsync(criteria, cancellationToken);
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetAccount endpoint");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching accounts.");
        }
    }
}