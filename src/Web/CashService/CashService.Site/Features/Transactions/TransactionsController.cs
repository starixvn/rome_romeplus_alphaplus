namespace AlphaPlus.CashService.Site.Features.Transactions;

using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.CashService.Site.Features.Shared.Filters;
using Core.Models;
using Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Models;
using Shared.Models;

[SiteAuthorizationFilter]
public class TransactionsController(
    HttpClientHelper httpClientHelper,
    IUserSession userSession,
    IHttpContextAccessor httpContextAccessor,
    IAntiforgery antiforgery)
    : SiteBaseController (userSession, antiforgery)
{
    private readonly int agentId = userSession.CustId;
    private readonly int custDbGroupId = userSession.CustDBGroupId;
    private readonly int siteDbGroupId = userSession.SiteDBGroupId;
    private readonly int userId = userSession.UserId;

    public IActionResult Index() => Ok("Transactions");

    public IActionResult Deposit() => View();

    public IActionResult Withdrawal() => View();

    public IActionResult History() => View();

    public async Task<IActionResult> GetHistory([FromQuery] GetTransactionFilter filter)
    {
        var url = $"/transactions/history";
        filter.CustId = agentId;
        filter.SiteDbGroupId = siteDbGroupId;
        var result = await httpClientHelper.GetAsync<object>(url, ObjectConverter.ToDictionary(filter));
        return Json(result);
    }

    public async Task<IActionResult> Detail(long transactionId, int custId)
    {
        var url = $"/transactions/detail?transactionId={transactionId}&custId={custId}&siteDbGroupId={siteDbGroupId}";
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
            { "paymentMethodId", paymentMethodId.ToString() },
            { "providerId", providerId.ToString() },
        };
        var url = "/withdrawal/limit-amount";
        var limitAmtDeposit = await httpClientHelper.GetAsync<object>(url, data);
        return Json(limitAmtDeposit);
    }

    public async Task<IActionResult> GetWithdrawalMemberCashBalance(int custId)
    {
        var url = $"/withdrawal/member-cash-balance?custId={custId}&siteDbGroupId={siteDbGroupId}";
        var balance = await httpClientHelper.GetAsync<object>(url);
        return Json(balance);
    }

    [HttpPost]
    public async Task<IActionResult> AddManualDeposit([FromForm] AddTransactionParams request)
    {
        if (userSession.IsInternal)
        {
            return Unauthorized();
        }

        request.UplineId = agentId;
        request.Ip = GetClientIp();
        request.SiteDbGroupId = siteDbGroupId;
        request.CustDbGroupId = custDbGroupId;
        request.PaySlip = request.Image != null ? await FileHelper.CompressAndConvertToBase64Async(request.Image) : null;
        var result = await httpClientHelper.PostAsync<AddTransactionParams, ResultModel>("/api/deposit/create", request);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddManualWithdrawal([FromForm] AddTransactionParams request)
    {
        if (userSession.IsInternal)
        {
            return Unauthorized();
        }

        var result = VerifyInputParam(request, true);
        if (result.Code != 0)
        {
            return Json(result);
        }
        request.UplineId = agentId;
        request.Ip = GetClientIp();
        request.SiteDbGroupId = siteDbGroupId;
        request.CustDbGroupId = custDbGroupId;
        request.PaySlip = request.Image != null ? await FileHelper.CompressAndConvertToBase64Async(request.Image) : null;
        result = await httpClientHelper.PostAsync<AddTransactionParams, ResultModel>("/api/withdrawal/create", request);
        return Json(result);
    }

    private static ResultModel VerifyInputParam(AddTransactionParams request, bool isWithdrawal)
    {
        var result = new ResultModel { Code = 0 };
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

        if (string.IsNullOrWhiteSpace(request.PaymentFrom) && (request.ProviderId <= 0 && isWithdrawal))
        {
            result.Code = 1;
            result.Message = "Please select PaymentFrom";
        }

        if (string.IsNullOrWhiteSpace(request.PaymentTo))
        {
            result.Code = 1;
            result.Message = "Please select PaymentTo";
        }

        if (!IsValidFile(request.Image))
        {
            result.Code = 1;
            result.Message = "File upload must be less than 1MB";
        }
        return result;
    }

    private static bool IsValidFile(IFormFile file)
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
    public async Task<IActionResult> RevertDepositTransactionToPending([FromBody] RevertTransaction request)
    {
        if (userSession.IsInternal)
        {
            return Unauthorized();
        }

        request.SiteDbGroupId = siteDbGroupId;
        request.CustDbGroupId = custDbGroupId;
        request.UserId = userId;
        request.Ip = GetClientIp();
        var result = await httpClientHelper.PostAsync<RevertTransaction, ResultModel>("/api/transactions/revert-pending-deposit", request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> RevertWithdrawalTransactionToPending([FromBody] RevertTransaction request)
    {
        if (userSession.IsInternal)
        {
            return Unauthorized();
        }

        request.SiteDbGroupId = siteDbGroupId;
        request.UserId = userId;
        request.Ip = GetClientIp();
        var result = await httpClientHelper.PostAsync<RevertTransaction, ResultModel>("/api/transactions/revert-pending-withdrawal", request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveDeposit([FromBody] ProcessTransactionModel request)
    {
        if (userSession.IsInternal)
        {
            return Unauthorized();
        }

        var result = await httpClientHelper.PostAsync<ProcessTransactionModel, ResultModel>("/deposit/approve", request);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> RejectDeposit([FromBody] ProcessTransactionModel request)
    {
        if (userSession.IsInternal)
        {
            return Unauthorized();
        }

        var result = await httpClientHelper.PostAsync<ProcessTransactionModel, ResultModel>("/deposit/reject", request);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveWithdrawal([FromBody] ProcessTransactionModel request)
    {
        if (userSession.IsInternal)
        {
            return Unauthorized();
        }

        if (request.Image != null)
        {
            request.Payslip = await FileHelper.CompressAndConvertToBase64Async(request.Image);
        }
        var result = await httpClientHelper.PostAsync<ProcessTransactionModel, ResultModel>("/withdrawal/approve", request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> RejectWithdrawal([FromBody] ProcessTransactionModel request)
    {
        if (userSession.IsInternal)
        {
            return Unauthorized();
        }

        if (request.Image != null)
        {
            request.Payslip = await FileHelper.CompressAndConvertToBase64Async(request.Image);
        }
        var result = await httpClientHelper.PostAsync<ProcessTransactionModel, ResultModel>("/withdrawal/reject", request);
        return Ok(result);
    }
}