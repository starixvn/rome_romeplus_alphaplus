namespace AlphaPlus.CashService.Site.Features.Transactions;

using Commands;
using Core.Models;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

public class TransactionsController(
    IConfiguration configuration,
    HttpClientHelper httpClientHelper,
    ILogger<TransactionsController> logger,
    IUserSession userSession)
    : Controller
{
    private readonly int agentId = userSession.CustId;
    private readonly string siteDbIp = userSession.AGIP;

    public IActionResult Index() => Ok("Transactions");

    public IActionResult Deposit() => View();

    public IActionResult Withdrawal() => View();

    public IActionResult History() => View();

    public async Task<IActionResult> GetAll(int transactionType, string status, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 100)
    {
        var url = $"/transactions?transactionType={transactionType}&status={status}&fromDate={fromDate}&toDate={toDate}&page={page}&pageSize={pageSize}";
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
        var url = $"/deposit/limit-amount?agentId={agentId}&custId={custId}&siteDbIp={siteDbIp}";
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
        var url = $"/transactions/get-members?agentId={agentId}&siteDbIp={siteDbIp}";
        var memberLogins = await httpClientHelper.GetAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashAgentBankInfo(int transactionType)
    {
        var url = $"/transactions/agent-bank-list?agentId={agentId}&transactionType={transactionType}&siteDbIp={siteDbIp}";
        var memberLogins = await httpClientHelper.GetAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashMemberBankInfo(int custId)
    {
        var url = $"/transactions/agent-bank-list?agentId={custId}&siteDbIp={siteDbIp}";
        var memberLogins = await httpClientHelper.GetAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashAgentProvider(int transactionType)
    {
        var url = $"/transactions/provider-bank-list?agentId={agentId}&transactionType={transactionType}&siteDbIp={siteDbIp}";
        var memberLogins = await httpClientHelper.GetAsync<object>(url);
        return Json(memberLogins);
    }

    [HttpPost]
    public async Task<IActionResult> RevertTransactionToPending([FromBody] RevertTransactionCommand request)
    {
        try
        {
            var url = "/transactions/revert";
            var result = await httpClientHelper.PostAsync<RevertTransactionCommand, ResultModel>(url, request);
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