namespace AlphaPlus.CashService.Site.Features.Transactions;

using Core.Models;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Shared.Models;

public class TransactionsController(
    HttpClientHelper httpClientHelper,
    ILogger<TransactionsController> logger,
    IUserSession userSession)
    : Controller
{
    private readonly int AgentId = userSession.CustId;
    private readonly int CustDbGroupId = userSession.CustDBGroupId;

    public IActionResult Index() => Ok("Transactions");

    public IActionResult Deposit() => View();

    public IActionResult Withdrawal() => View();

    public IActionResult History() => View();

    public async Task<IActionResult> GetAll([FromQuery] GetTransactionFilter filter)
    {
        var url = $"/transactions?transactionType={filter.TransactionType}&status={filter.StatusId}&fromDate={filter.FromDate}&toDate={filter.ToDate}&page={filter.PageIndex}&pageSize={filter.PageSize}";
        var result = await httpClientHelper.GetAsync<object>(url);
        return Json(result);
    }

    public async Task<IActionResult> Detail(int transactionId)
    {
        var url = $"/transactions/{transactionId}";
        var result = await httpClientHelper.GetAsync<object>(url);
        return Ok(result);
    }

    public async Task<IActionResult> GetDepositTransactionTypes()
    {
        var url = "/deposit/transaction-types";
        var transactionTypes = await httpClientHelper.GetAsync<object>(url);
        return Json(transactionTypes);
    }

    public async Task<IActionResult> GetMemberBankAccounts()
    {
        var url = "/deposit/member-bank-accounts";
        var memberAccounts = await httpClientHelper.GetAsync<object>(url);
        return Json(memberAccounts);
    }

    public async Task<IActionResult> GetLimitAmtDeposit(int custId)
    {
        var url = $"/deposit/limit-amount?agentId={AgentId}&custId={custId}&custDbGroupId={CustDbGroupId}";
        var limitAmtDeposit = await httpClientHelper.GetAsync<object>(url);
        return Json(limitAmtDeposit);
    }

    public async Task<IActionResult> AddManualDeposit([FromBody] object request)
    {
        var result = await httpClientHelper.PostAsync<object, ResultModel>("/deposit/create", request);
        return Json(result);
    }

    public async Task<IActionResult> GetBankWithdrawalAccounts()
    {
        var url = "/withdrawal/bank-withdrawal-accounts";
        var withdrawalAccounts = await httpClientHelper.GetAsync<object>(url);
        return Json(withdrawalAccounts);
    }

    public async Task<IActionResult> GetMemberLoginName()
    {
        var url = $"/transactions/get-members?agentId={AgentId}&custDbGroupId={CustDbGroupId}";
        var memberLogins = await httpClientHelper.GetAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashAgentBankInfo(int transactionType)
    {
        var url = $"/transactions/agent-bank-list?agentId={AgentId}&transactionType={transactionType}&custDbGroupId={CustDbGroupId}";
        var memberLogins = await httpClientHelper.GetAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashMemberBankInfo(int custId)
    {
        var url = $"/transactions/agent-bank-list?agentId={custId}&custDbGroupId={CustDbGroupId}";
        var memberLogins = await httpClientHelper.GetAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashAgentProvider(int transactionType)
    {
        var url = $"/transactions/provider-bank-list?agentId={AgentId}&transactionType={transactionType}&custDbGroupId={CustDbGroupId}";
        var memberLogins = await httpClientHelper.GetAsync<object>(url);
        return Json(memberLogins);
    }

    [HttpPost]
    public async Task<IActionResult> RevertTransactionToPending([FromBody] RevertTransaction request)
    {
        try
        {
            var result = await httpClientHelper.PostAsync<RevertTransaction, ResultModel>("/transactions/revert", request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reverting transaction");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Code = StatusCodes.Status500InternalServerError, Message = "An unexpected error occurred" });
        }
    }
}