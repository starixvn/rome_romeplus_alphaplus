namespace AlphaPlus.CashService.Site.Features.Transactions;

using AlphaPlus.Shared.AppResources.CashServices;
using Core.Models;
using Fanex.Customer.Client.Services;
using Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Models;
using Payments.Models;
using Services;
using Shared;
using Shared.Filters;
using Shared.Models;
using Shared.Resources;

[SiteAuthorizationFilter(PageAccessType.CashService)]
public class TransactionsController(
    HttpClientHelper httpClientHelper,
    IUserSession userSession,
    ITransactionHistoryWorkbook transactionHistoryWorkbook,
    IAntiforgery antiforgery,
    HttpClient httpClient,
    ICustomerService customerService)
    : SiteBaseController(userSession, antiforgery, httpClient, customerService)
{
    private readonly int agentId = userSession.CustId;
    private readonly int custDbGroupId = userSession.CustDBGroupId;
    private readonly int siteDbGroupId = userSession.SiteDBGroupId;

    public IActionResult Index() => Ok("Transactions");

    private async Task<IActionResult> RenderCashServiceViewAsync()
    {
        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        return View(new SiteBaseViewModel { IsCashServiceEnabled = isCashServiceEnabled });
    }

    public async Task<IActionResult> Deposit()
    {
        return await RenderCashServiceViewAsync();
    }

    public async Task<IActionResult> Withdrawal()
    {
        return await RenderCashServiceViewAsync();
    }

    public async Task<IActionResult> History()
    {
        return await RenderCashServiceViewAsync();
    }

    public async Task<IActionResult> GetHistory([FromQuery] GetTransactionFilter filter)
    {
        const string url = "/transactions/history";
        filter.CustId = agentId;
        filter.SiteDbGroupId = siteDbGroupId;
        var result = await httpClientHelper.GetAsync<TransactionResponse>(url, ObjectConverter.ToDictionary(filter));
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> ExportExcel([FromForm] ExportExcelParams param)
    {
        var filter = new GetTransactionFilter
        {
            FromDate = param.FromDate,
            ToDate = param.ToDate,
            PaymentStatus = param.Status,
            TransactionType = param.TransactionType,
            SiteDbGroupId = siteDbGroupId,
            CustId = agentId,
            Account = param.Account
        };
        param.Username = _userSession.UserName;
        var response = await httpClientHelper.GetAsync<TransactionResponse>("/transactions/history", ObjectConverter.ToDictionary(filter));
        var excelFileViewModel = transactionHistoryWorkbook.BuildExcelFile(param, response.Items);
        return File(excelFileViewModel.Content, excelFileViewModel.ContentType, excelFileViewModel.Name);
    }

    public async Task<IActionResult> Detail(long transactionId, int custId)
    {
        var url = $"/transactions/detail?transactionId={transactionId}&custId={custId}&siteDbGroupId={siteDbGroupId}";
        var result = await httpClientHelper.GetAsync<Transaction>(url);
        return Ok(result);
    }

    public async Task<IActionResult> GetDepositTransactionTypes()
    {
        const string url = "/deposit/transaction-types";
        var transactionTypes = await httpClientHelper.GetAsync<object>(url);
        return Json(transactionTypes);
    }

    public async Task<IActionResult> GetMemberBankAccounts()
    {
        const string url = "/deposit/member-bank-accounts";
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
        if (_userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        if (isCashServiceEnabled == 0)
        {
            return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
        }

        request.UplineId = agentId;
        request.SubAccId = _userSession.SubAccId;
        request.Ip = _userSession.UserLoginIp;
        request.SiteDbGroupId = siteDbGroupId;
        request.CustDbGroupId = custDbGroupId;
        request.PaySlip = request.Image != null ? await FileHelper.CompressAndConvertToBase64Async(request.Image) : null;
        var result = await httpClientHelper.PostAsync<AddTransactionParams, ResultModel>("/api/deposit/create", request);
        if (result.Code != 0)
        {
            result.Message = ErrorCodeHandler.MappingAddManualDepositErrorCode(result);
        }
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddManualWithdrawal([FromForm] AddTransactionParams request)
    {
        if (_userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        if (isCashServiceEnabled == 0)
        {
            return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
        }

        var result = VerifyInputParam(request, true);
        if (request.ProviderId > 0 && !string.IsNullOrWhiteSpace(request.PaymentTo))
        {
            var bankCode = request.PaymentTo.Split('-')[0].Trim();
            result = await ValidateProviderBankSupported(request.ProviderId, bankCode, request.PaymentFrom.Split("-")[1]);
        }

        if (result.Code != 0)
        {
            return Json(result);
        }

        request.UplineId = agentId;
        request.SubAccId = _userSession.SubAccId;
        request.Ip = _userSession.UserLoginIp;
        request.SiteDbGroupId = siteDbGroupId;
        request.CustDbGroupId = custDbGroupId;
        request.PaySlip = request.Image != null ? await FileHelper.CompressAndConvertToBase64Async(request.Image) : null;
        result = await httpClientHelper.PostAsync<AddTransactionParams, ResultModel>("/api/withdrawal/create", request);
        if (result.Code != 0)
        {
            result.Message = ErrorCodeHandler.MappingAddManualWithdrawalErrCode(result, request.CustId, agentId);
        }
        return Json(result);
    }

    private static ResultModel VerifyInputParam(AddTransactionParams request, bool isWithdrawal)
    {
        var result = new ResultModel { Code = 0 };
        if (request.Amount <= 0 || (request.MaxLimitAmt > 0 && request.Amount > request.MaxLimitAmt))
        {
            result.Code = 1;
            result.Message = CashServicesResx.TheAmountExceedsTheAllowedMinimumOrMaximumValue;
        }
        if (request.CustId <= 0)
        {
            result.Code = 1;
            result.Message = CashServicesResx.MemberUsernameIsRequired;
        }

        if (string.IsNullOrWhiteSpace(request.PaymentFrom) && (request.ProviderId <= 0 && isWithdrawal))
        {
            result.Code = 1;
            result.Message = CashServicesResx.FromIsRequired;
        }

        if (string.IsNullOrWhiteSpace(request.PaymentTo))
        {
            result.Code = 1;
            result.Message = CashServicesResx.ToIsRequired;
        }

        if (!IsValidFile(request.Image))
        {
            result.Code = 1;
            result.Message = CashServicesResx.FileUploadMustBeLessThan1MB;
        }
        return result;
    }

    private async Task<ResultModel> ValidateProviderBankSupported(int providerId, string bankCode, string providerName)
    {
        var url = $"/common/banks?site={_userSession.Site}&providerId={providerId}&languageCode={_userSession.Language}";
        var supportedBanks = await httpClientHelper.GetAsync<List<BankModel>>(url);
        if (supportedBanks == null || !supportedBanks.Exists(b => b.Code == bankCode))
        {
            return new ResultModel
            {
                Code = 1,
                Message = string.Format(CashServicesResx.SelectedBankIsNotSupportedByProvider, providerName)
            };
        }

        return new ResultModel(0, string.Empty);
    }

    private static bool IsValidFile(IFormFile file)
    {
        if (file == null) return true;
        const long maxFileSize = 1 * 1024 * 1024;

        return file.Length <= maxFileSize;
    }

    public async Task<IActionResult> GetBankWithdrawalAccounts()
    {
        const string url = "/withdrawal/bank-withdrawal-accounts";
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
        if (_userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        if (isCashServiceEnabled == 0)
        {
            return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
        }
        request.SetUserSession(_userSession);
        var result = await httpClientHelper.PostAsync<RevertTransaction, ResultModel>("/api/deposit/revert-pending", request);
        if (result.Code != 0)
        {
            result.Message = ErrorCodeHandler.MappingRevertTransactionErrCode(result, request.TransId.ToString(), request.CustId, request.ActorId);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> RevertWithdrawalTransactionToPending([FromBody] RevertTransaction request)
    {
        if (_userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        if (isCashServiceEnabled == 0)
        {
            return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
        }

        request.SetUserSession(_userSession);
        var result = await httpClientHelper.PostAsync<RevertTransaction, ResultModel>("/api/withdrawal/revert-pending", request);
        if (result.Code != 0)
        {
            result.Message = ErrorCodeHandler.MappingRevertTransactionErrCode(result, request.TransId.ToString(), request.CustId, request.ActorId);
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveDeposit([FromBody] ProcessDepositTransaction request)
    {
        if (_userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        if (isCashServiceEnabled == 0)
        {
            return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
        }

        request.SetUserSession(_userSession);
        var result = await httpClientHelper.PostAsync<ProcessDepositTransaction, ResultModel>("/api/deposit/approve", request);
        if (result.Code != 0)
        {
            result.Message = ErrorCodeHandler.MappingUpdateTransactionStatus(result, request.TransId.ToString(), request.CustId, request.ActorId);
        }
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> RejectDeposit([FromBody] ProcessDepositTransaction request)
    {
        if (_userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }

        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        if (isCashServiceEnabled == 0)
        {
            return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
        }

        request.SetUserSession(_userSession);

        var result = await httpClientHelper.PostAsync<ProcessDepositTransaction, ResultModel>("/api/deposit/reject", request);
        if (result.Code != 0)
        {
            result.Message = ErrorCodeHandler.MappingUpdateTransactionStatus(result, request.TransId.ToString(), request.CustId, request.ActorId);
        }
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveWithdrawal([FromForm] ProcessWithdrawalTransaction request)
    {
        if (_userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }
        request.SetUserSession(_userSession);

        if (request.Image != null)
        {
            request.Payslip = await FileHelper.CompressAndConvertToBase64Async(request.Image);
        }
        var result = await httpClientHelper.PostAsync<ProcessWithdrawalTransaction, ResultModel>("/api/withdrawal/approve", request);
        if (result.Code != 0)
        {
            result.Message = ErrorCodeHandler.MappingUpdateTransactionStatus(result, request.TransId.ToString(), request.CustId, request.ActorId);
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> RejectWithdrawal([FromForm] ProcessWithdrawalTransaction request)
    {
        if (_userSession.IsInternal)
        {
            return Json(new ResultModel(401, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.NotAuthorized));
        }
        request.SetUserSession(_userSession);

        if (request.Image != null)
        {
            request.Payslip = await FileHelper.CompressAndConvertToBase64Async(request.Image);
        }
        var result = await httpClientHelper.PostAsync<ProcessWithdrawalTransaction, ResultModel>("/api/withdrawal/reject", request);
        if (result.Code != 0)
        {
            result.Message = ErrorCodeHandler.MappingUpdateTransactionStatus(result, request.TransId.ToString(), request.CustId, request.ActorId);
        }
        return Ok(result);
    }
}