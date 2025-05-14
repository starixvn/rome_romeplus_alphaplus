namespace AlphaPlus.CashService.Site.Features.Transactions;

using AlphaPlus.CashService.Site.Features.Shared.Models;
using Commands;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries;
using Requests;
using Shared;

public class TransactionsController(
    IMediator mediator,
    IConfiguration configuration,
    HttpClient httpClient,
    ILogger<TransactionsController> logger,
    IUserSession userSession
) : SiteBaseController(mediator)
{
    private readonly string _apiBaseUrl = configuration["CashServiceAPIUrl"] ?? throw new InvalidOperationException("CashServiceAPIUrl not configured.");
    private readonly int agentId = userSession.CustId;
    private readonly string siteDbIp = userSession.AGIP;

    public IActionResult Index() => Ok("Transactions");

    public IActionResult Deposit() => View();

    public IActionResult Withdrawal() => View();

    public IActionResult History() => View();

    public async Task<IActionResult> GetAll(int transactionType,
        string status, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 100)
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
        var query = new GetTransactionByIdQuery(transactionId);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    public async Task<IActionResult> GetDepositTransactionTypes()
    {
        var url = $"{_apiBaseUrl}/deposit/transaction-types";
        var transactionTypes = await httpClient.GetFromJsonAsync<object>(url);
        return Json(transactionTypes);
    }

    public async Task<IActionResult> GetMemberBankAccounts()
    {
        var url = $"{_apiBaseUrl}/deposit/member-bank-accounts";
        var memberAccounts = await httpClient.GetFromJsonAsync<object>(url);
        return Json(memberAccounts);
    }

    public async Task<IActionResult> GetLimitAmtDeposit(int custId)
    {
        var url = $"{_apiBaseUrl}/deposit/limit-amount?agentId={agentId}&custId={custId}&siteDbIp={siteDbIp}";
        var limitAmtDeposit = await httpClient.GetFromJsonAsync<object>(url);
        return Json(limitAmtDeposit);
    }

    public async Task<IActionResult> AddManualDeposit([FromBody] CreateManualRequest request)
    {
        request.SiteDbIp = siteDbIp;
        request.SetUserSession(userSession);
        var url = $"{_apiBaseUrl}/deposit/create";
        var response = await httpClient.PostAsJsonAsync(url, request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<object>();
        return Json(result);
    }

    public async Task<IActionResult> GetBankWithdrawalAccounts()
    {
        var url = $"{_apiBaseUrl}/withdrawal/bank-withdrawal-accounts";
        var withdrawalAccounts = await httpClient.GetFromJsonAsync<object>(url);
        return Json(withdrawalAccounts);
    }

    public async Task<IActionResult> GetMemberLoginName()
    {
        var url = $"{_apiBaseUrl}/transactions/get-members/{agentId}/{siteDbIp}";
        var memberLogins = await httpClient.GetFromJsonAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashAgentBankInfo(int transactionType)
    {
        var url = $"{_apiBaseUrl}/common/agent-bank-list?agentId={agentId}&transactionType={transactionType}&siteDbIp={siteDbIp}";
        var memberLogins = await httpClient.GetFromJsonAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashMemberBankInfo(int custId)
    {
        var url = $"{_apiBaseUrl}/common/agent-bank-list/{custId}/{siteDbIp}";
        var memberLogins = await httpClient.GetFromJsonAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashAgentProvider(int transactionType)
    {
        var url = $"{_apiBaseUrl}/common/provider-bank-list?agentId={agentId}&transactionType={transactionType}&siteDbIp={siteDbIp}";
        var memberLogins = await httpClient.GetFromJsonAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetFilterAccount(int transactionType)
    {
        try
        {
            var query = new GetFilterAccountQuery(transactionType);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting filter accounts");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResultModel { Code = StatusCodes.Status500InternalServerError, Message = "Failed to fetch accounts." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RevertTransactionToPending([FromBody] RevertTransactionRequest request)
    {
        if (request.TransactionId <= 0)
        {
            return BadRequest(new ResultModel { Code = StatusCodes.Status400BadRequest, Message = "Invalid TransactionId." });
        }

        try
        {
            var command = new RevertTransactionCommand(request.TransactionId, request.Remarks);
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