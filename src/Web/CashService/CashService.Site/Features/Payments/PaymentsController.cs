using AlphaPlus.CashService.Site.Features.Payments.Models;
using AlphaPlus.CashService.Site.Features.Payments.Requests;
using AlphaPlus.CashService.Site.Features.Payments.ViewModels;
using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.CashService.Site.Features.Shared.Filters;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Payments
{
    [SiteAuthorizationFilter(PageAccessType.CashService)]
    public class PaymentsController : SiteBaseController
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;

        public PaymentsController(
            IConfiguration configuration,
            HttpClient httpClient,
            IUserSession userSession,
            IAntiforgery antiforgery) : base(userSession, antiforgery)
        {
            _apiBaseUrl = configuration["CashService:APIUrl"];
            _httpClient = httpClient;
        }

        private int _custId => _userSession.CustId;

        public IActionResult Index()
        {
            var viewModel = new PaymentBaseViewModel
            {
                Language = _userSession.Language,
                DarkModeName = _userSession.DarkModeName
            };

            return View(viewModel);
        }

        public async Task<IActionResult> GetProviders(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/providers-by-custid/{_custId}?languageCode={_userSession.Language}";
            var providers = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(providers);
        }

        public async Task<IActionResult> GetDailyWithdrawalCount(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/get-daily-withdrawal-count/{_custId}";
            var result = await _httpClient.GetFromJsonAsync<int>(url, cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SetDailyWithdrawalCount(
            [FromBody] SetDailyWithdrawalCountRequest request,
            CancellationToken cancellationToken)
            => await DoAction<object>($"{_apiBaseUrl}/payments/set-daily-withdrawal-count", request, cancellationToken);

        public async Task<IActionResult> GetProviderFees(int providerId, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/provider-fees/{providerId}?languageCode={_userSession.Language}";
            var fees = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(fees);
        }

        public async Task<IActionResult> ManageProviderSettings(int id, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/providers/{_custId}/{id}?languageCode={_userSession.Language}";
            var provider = await _httpClient.GetFromJsonAsync<ProviderModel>(url, cancellationToken);
            var addPaymentMethodViewModel = await _httpClient.GetFromJsonAsync<AddPaymentMethodViewModel>($"{_apiBaseUrl}/payments/get-add-payment-method-info?custId={_custId}&providerId={provider.Id}&transactionType=0&languageCode={_userSession.Language}", cancellationToken);
            var allPaymentMethods = addPaymentMethodViewModel.PaymentMethods.ToList().Prepend(PaymentMethodModel.CreateOptionAll());

            var viewModel = new ProviderSettingsViewModel
            {
                Provider = provider,
                AddPaymentMethodViewModel = new AddPaymentMethodViewModel
                {
                    PaymentMethods = allPaymentMethods,
                    ChannelContact = addPaymentMethodViewModel.ChannelContact
                },
                Language = _userSession.Language,
                DarkModeName = _userSession.DarkModeName
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ProviderSettings(int providerId, int method, int status, int transactionType, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/provider-settings?providerId={providerId}&custId={_custId}&method={method}&status={status}&transactionType={transactionType}&languageCode={_userSession.Language}";
            var settings = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(settings);
        }

        public async Task<IActionResult> GetAddPaymentMethodInfo(int providerId, int transactionType, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/get-add-payment-method-info?custId={_custId}&providerId={providerId}&transactionType={transactionType}&languageCode={_userSession.Language}";
            var paymentMethods = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(paymentMethods);
        }

        [HttpPost]
        public async Task<IActionResult> AddPaymentMethod(
            [FromBody] InsertPaymentMethodRequest request,
            CancellationToken cancellationToken)
            => await DoAction<object>($"{_apiBaseUrl}/payments/add-payment-method", request, cancellationToken);

        [HttpPost]
        public async Task<IActionResult> UpdatePaymentMethod(
            [FromBody] UpdatePaymentMethodRequest request,
            CancellationToken cancellationToken)
            => await DoAction<object>($"{_apiBaseUrl}/payments/update-payment-method", request, cancellationToken);

        public async Task<IActionResult> GetBanksByTransactionType(int transactionType, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/banks?site={_userSession.Site}&transactionType={transactionType}&languageCode={_userSession.Language}";
            var banks = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(banks);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProvider(
            [FromBody] UpdateProviderRequest request,
            CancellationToken cancellationToken)
            => await DoAction<object>($"{_apiBaseUrl}/payments/update-provider", request, cancellationToken);

        public async Task<IActionResult> GetBanksByCust(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/banks/{_custId}?languageCode={_userSession.Language}";
            var banks = await _httpClient.GetFromJsonAsync<IEnumerable<BankModel>>(url, cancellationToken);
            return Json(banks);
        }

        public async Task<IActionResult> GetTransactionTypes(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/transaction-types";
            var transactionTypes = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(transactionTypes);
        }

        public async Task<IActionResult> GetBankAccounts(
            int bankId,
            int transactionType,
            int status,
            string searchText,
            CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/bank-accounts?custId={_custId}&bankId={bankId}&transactionType={transactionType}&status={status}&searchText={searchText}&languageCode={_userSession.Language}";
            var bankAccounts = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(bankAccounts);
        }

        public async Task<IActionResult> GetTransactionLimit(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/transaction-limit/{_custId}";
            var result = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SetTransactionLimit(
            [FromBody] SetTransactionLimitRequest request,
            CancellationToken cancellationToken)
            => await DoAction<object>($"{_apiBaseUrl}/payments/set-transaction-limit", request, cancellationToken);

        [HttpPost]
        public async Task<IActionResult> AddBankAccount(
            [FromBody] AddBankAccountRequest request,
            CancellationToken cancellationToken)
            => await DoAction<object>($"{_apiBaseUrl}/payments/add-bank-account", request, cancellationToken);

        public async Task<IActionResult> GetBankAccountById(
            int bankAccountId,
            CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/bank-accounts/{bankAccountId}?languageCode={_userSession.Language}";
            var bankAccount = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(bankAccount);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBankAccount(
            [FromBody] UpdateBankAccountRequest request,
            CancellationToken cancellationToken)
            => await DoAction<object>($"{_apiBaseUrl}/payments/update-bank-account", request, cancellationToken);

        [HttpPost]
        public async Task<IActionResult> DeleteBankAccount(
            [FromBody] DeleteBankAccountRequest request,
            CancellationToken cancellationToken)
            => await DoAction<object>($"{_apiBaseUrl}/payments/bank-accounts/delete", request, cancellationToken);

        public async Task<IActionResult> GetCountryCodes(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/common/country-codes/{_userSession.Site}";
            var countryCodes = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(countryCodes);
        }

        private async Task<IActionResult> DoAction<TResult>(
            string url,
            Shared.Requests.BaseActionRequest request,
            CancellationToken cancellationToken)
        {
            if (_userSession.IsInternal)
            {
                return Unauthorized();
            }

            request.SetUserSession(_userSession);
            var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken);
            return Json(result);
        }
    }
}