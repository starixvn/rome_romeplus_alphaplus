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

    [HttpGet("history/{id}")]
    public async Task<ActionResult<TransactionModel>> GetTransactionById(int id)
    {
        var transaction = await transactionService.GetTransactionByIdAsync(id);
        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }
}