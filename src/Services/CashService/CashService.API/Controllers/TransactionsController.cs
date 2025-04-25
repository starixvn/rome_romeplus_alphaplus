namespace AlphaPlus.CashService.API.Controllers;

using Criteria.Transactions;
using Microsoft.AspNetCore.Mvc;
using Models.Transactions;
using Services;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(ITransactionService transactionService)
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
}