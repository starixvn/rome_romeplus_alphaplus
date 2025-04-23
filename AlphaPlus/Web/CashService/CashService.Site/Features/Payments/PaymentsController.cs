using Microsoft.AspNetCore.Mvc;
using AlphaPlus.CashService.Site.Helpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using AlphaPlus.CashService.Site.Features.Payments.Requests;
using AlphaPlus.CashService.Site.Features.Payments.Models;

namespace AlphaPlus.CashService.Site.Features.Payments
{
    public class PaymentsController : Controller
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly APIHelper _apiHelper;
        private readonly string _apiBaseUrl;
        private readonly int _custId;
        private readonly HttpClient _httpClient;

        public PaymentsController(
            ILogger<PaymentsController> logger, 
            IWebHostEnvironment environment, 
            APIHelper apiHelper, 
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _logger = logger;
            _environment = environment;
            _apiHelper = apiHelper;
            _apiBaseUrl = configuration["CashServiceAPIUrl"];
            _httpClient = httpClient;

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

        public async Task<IActionResult> GetProviderFees()
        {
            var url = $"{_apiBaseUrl}/payments/provider-fees/{_custId}";
            var fees = await _apiHelper.GetAsync<object>(url);
            return Json(fees);
        }

        public async Task<IActionResult> PaymentProviderSettings(int id)
        {
            var url = $"{_apiBaseUrl}/payment-providers/{id}";
            // var provider = await _apiHelper.GetAsync<ProviderModel>(url);
            var provider = new ProviderModel {
                Id = 1,
                Name = "Credit Card",
                Status = 1,
                LastUpdated = DateTime.Now.AddDays(-1)
            };

            return View(provider);
        }

    }
}