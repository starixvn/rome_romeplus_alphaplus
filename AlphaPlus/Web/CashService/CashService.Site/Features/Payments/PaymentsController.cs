using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using AlphaPlus.CashService.Site.Features.Payments.Requests;
using AlphaPlus.CashService.Site.Features.Payments.Models;
using AlphaPlus.CashService.Site.Features.Payments.ViewModels;
using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Payments
{
    public class PaymentsController : Controller
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly string _apiBaseUrl;
        private readonly int _custId;
        private readonly HttpClient _httpClient;
        private readonly IUserSession _userSession;

        public PaymentsController(
            ILogger<PaymentsController> logger, 
            IWebHostEnvironment environment, 
            IConfiguration configuration,
            HttpClient httpClient,
            IUserSession userSession)
        {
            _logger = logger;
            _environment = environment;
            _apiBaseUrl = configuration["CashServiceAPIUrl"];
            _httpClient = httpClient;
            _userSession = userSession;

            _custId = 1176086;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetProviders(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/providers-by-custid/{_custId}";
            var providers = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(providers);
        }

        public async Task<IActionResult> GetDailyWithdrawalCount(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/get-daily-withdrawal-count/{_custId}";
            var result = await _httpClient.GetFromJsonAsync<int>(url, cancellationToken);
            return Json(result);
        }

        public async Task<IActionResult> SetDailyWithdrawalCount(
            [FromBody] SetDailyWithdrawalCountRequest request,
            CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/set-daily-withdrawal-count";
            request.CustId = _custId;

            var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<object>(cancellationToken: cancellationToken);
            return Json(result);
        }

        public async Task<IActionResult> GetProviderFees(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/provider-fees/{_custId}";
            var fees = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(fees);
        }

        public async Task<IActionResult> ManageProviderSettings(int id, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/providers/{_custId}/{id}";
            var provider = await _httpClient.GetFromJsonAsync<ProviderModel>(url, cancellationToken);
            var paymentMethods = await _httpClient.GetFromJsonAsync<IEnumerable<PaymentMethodModel>>($"{_apiBaseUrl}/payments/payment-methods", cancellationToken);
            var allPaymentMethods = paymentMethods.ToList().Prepend(PaymentMethodModel.CreateOptionAll());

            var viewModel = new ProviderSettingsViewModel
            {
                Provider = provider,
                PaymentMethods = allPaymentMethods
            };
            
            return View(viewModel);
        }
    
        public async Task<IActionResult> ProviderSettings(int providerId,int method, int status, int transactionType, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/provider-settings?providerId={providerId}&method={method}&status={status}&transactionType={transactionType}";
            var settings = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(settings);
        }

        public async Task<IActionResult> GetPaymentMethods(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/payment-methods";
            var paymentMethods = await _httpClient.GetFromJsonAsync<IEnumerable<PaymentMethodModel>>(url, cancellationToken);
            var allPaymentMethods = paymentMethods.ToList().Prepend(PaymentMethodModel.CreateOptionAll());
            return Json(allPaymentMethods);
        }

        public async Task<IActionResult> AddPaymentMethod(
            [FromBody] PaymentMethodModel paymentMethod,
            CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/add-payment-method";
            paymentMethod.CustId = _custId;

            var response = await _httpClient.PostAsJsonAsync(url, paymentMethod, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<object>(cancellationToken: cancellationToken);
            return Json(result);
        }

        public async Task<IActionResult> GetBanks(int transactionType, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/banks?transactionType={transactionType}";
            var banks = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(banks);
        }

        public async Task<IActionResult> UpdateProvider(
            [FromBody] ProviderModel provider,
            CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/payments/update-provider";
            provider.CustId = _custId;

            var response = await _httpClient.PostAsJsonAsync(url, provider, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<object>(cancellationToken: cancellationToken);
            return Json(result);
        }
    }
}