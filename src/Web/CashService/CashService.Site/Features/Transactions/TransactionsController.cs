using Microsoft.AspNetCore.Mvc.Formatters;

namespace AlphaPlus.CashService.Site.Features.Transactions;

using Core.Models;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Shared.Models;

public class TransactionsController(
    HttpClientHelper httpClientHelper,
    ILogger<TransactionsController> logger,
    IUserSession userSession,
    IHttpContextAccessor httpContextAccessor)
    : Controller
{
    private readonly int agentId = userSession.CustId;
    private readonly int custDbGroupId = userSession.CustDBGroupId;
    private readonly int siteDbGroupId = userSession.SiteDBGroupId;

    public IActionResult Index() => Ok("Transactions");

    public IActionResult Deposit() => View();

    public IActionResult Withdrawal() => View();

    public IActionResult History() => View();

    public async Task<IActionResult> GetAll([FromQuery] GetTransactionFilter filter)
    {
        var url = $"/transactions/history?transactionType={filter.TransactionType}&paymentStatus={filter.PaymentStatus}&fromDate={filter.FromDate}&toDate={filter.ToDate}&pageIndex={filter.PageIndex}&pageSize={filter.PageSize}";
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
        var url = $"/deposit/limit-amount?agentId={agentId}&custId={custId}&custDbGroupId={custDbGroupId}";
        var limitAmtDeposit = await httpClientHelper.GetAsync<object>(url);
        return Json(limitAmtDeposit);
    }

    public async Task<IActionResult> GetSettingLimitAmtWithdrawal(int custId, int providerId, int paymentMethodId)
    {
        var data = new Dictionary<string, string> {
            { "uplineId", agentId.ToString() },
            { "custId", custId.ToString() },
            { "custDbGroupId", custDbGroupId.ToString() },
            { "providerId", providerId.ToString() },
            { "paymentMethodId", paymentMethodId.ToString() }
        };
        var url = $"/withdrawal/limit-amount";
        var limitAmtDeposit = await httpClientHelper.GetAsync<object>(url, data);
        return Json(limitAmtDeposit);
    }

    public async Task<IActionResult> AddManualDeposit([FromBody] AddWithdrawalParams request)
    {
        var rq = new AddWithdrawalParams
        {
            UplineId = agentId,
            CustId = 1176092
        };

        var result = VerifyInputParam(request);
        if (result.Code != 0)
        {
            return Json(result);
        }
        request.UplineId = agentId;
        request.Ip = GetClientIp();
        request.Payslip = request.File != null ? await ToByteArrayAsync(request.File) : null;
        result = await httpClientHelper.PostAsync<AddWithdrawalParams, ResultModel>("/deposit/create", rq);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddManualWithdrawal([FromBody] AddWithdrawalParams request)
    {
        var result = VerifyInputParam(request);
        if (result.Code != 0)
        {
            return Json(result);
        }
        request.UplineId = agentId;
        request.Ip = GetClientIp();
        request.SiteDbGroupId = siteDbGroupId;
        request.CustDbGroupId = custDbGroupId;
        request.Payslip = request.File != null ? await FileHelper.CompressAndConvertToBase64Async(request.File) : null;
        result = await httpClientHelper.PostAsync<AddWithdrawalParams, ResultModel>("/withdrawal/create", request);
        return Json(result);
    }

    public async Task<byte[]> ToByteArrayAsync(IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }

    private static ResultModel VerifyInputParam(AddWithdrawalParams request)
    {
        var result = new ResultModel();
        if (request.Amount <= 0 || (request.MaxLimitAmt > 0 && request.Amount > request.MaxLimitAmt))
        {
            result.Code = 1;
            result.Message = "Amount must be greater than 0 and less than MaxLimit";
        }
        if (request.CustId <= 0)
        {
            result.Code = 1;
            result.Message = "Please select member login";
        }

        if (string.IsNullOrWhiteSpace(request.PaymentFrom) && request.ProviderId <= 0)
        {
            result.Code = 1;
            result.Message = "Please select From Payment";
        }

        if (string.IsNullOrWhiteSpace(request.PaymentTo))
        {
            result.Code = 1;
            result.Message = "Please select Member Bank account";
        }

        if (!IsValidFile(request.File))
        {
            result.Code = 1;
            result.Message = "File upload must be less than 1MB";
        }
        return result;
    }

    private static bool IsValidFile(IFormFile? file)
    {
        if (file == null) return true;
        const long maxFileSize = 1 * 1024 * 1024;

        return file.Length <= maxFileSize;
    }

    private string GetClientIp()
    {
        var context = httpContextAccessor.HttpContext;

        var ip = context?.Request?.Headers["X-Forwarded-For"].FirstOrDefault();

        if (string.IsNullOrEmpty(ip))
        {
            ip = context?.Connection?.RemoteIpAddress?.ToString();
        }

        return ip ?? "Unknown";
    }

    public async Task<IActionResult> GetBankWithdrawalAccounts()
    {
        var url = "/withdrawal/bank-withdrawal-accounts";
        var withdrawalAccounts = await httpClientHelper.GetAsync<object>(url);
        return Json(withdrawalAccounts);
    }

    public async Task<IActionResult> GetMemberLoginName()
    {
        var url = $"/transactions/get-members?agentId={agentId}&custDbGroupId={custDbGroupId}";
        var memberLogins = await httpClientHelper.GetAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashAgentBankInfo(int transactionType)
    {
        var url = $"/transactions/agent-bank-list?agentId={agentId}&transactionType={transactionType}&custDbGroupId={custDbGroupId}";
        var memberLogins = await httpClientHelper.GetAsync<object>(url);
        return Json(memberLogins);
    }

    public async Task<IActionResult> GetCashMemberBankInfo(int custId)
    {
        var url = $"/transactions/member-bank-list?custId={custId}&custDbGroupId={custDbGroupId}";
        var memberBanks = await httpClientHelper.GetAsync<object>(url);
        return Json(memberBanks);
    }

    public async Task<IActionResult> GetCashAgentProvider(int transactionType)
    {
        var url = $"/transactions/payment-provider-list?agentId={agentId}&transactionType={transactionType}&custDbGroupId={custDbGroupId}";
        var agentProviders = await httpClientHelper.GetAsync<object>(url);
        return Json(agentProviders);
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