using Microsoft.AspNetCore.Mvc;
using AlphaPlus.CashService.Site.Features.Payments.Models;
using AlphaPlus.CashService.Site.Helpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

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
            var url = $"{_apiBaseUrl}/payment-providers-by-custid/{_custId}";
            var providers = await _httpClient.GetFromJsonAsync<List<ProviderModel>>(url, cancellationToken);
            return Json(providers);
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

        public async Task<IActionResult> GetProviderFees(int id)
        {
            var url = $"{_apiBaseUrl}/payment-provider-fees/{id}";
            var fees = await _apiHelper.GetAsync<List<ProviderFeeGroupModel>>(url);
            return Json(fees);
        }

        public async Task<IActionResult> SetDailyWithdrawalCount(int custId, int count)
        {
            var url = $"{_apiBaseUrl}/set-daily-withdrawal-count";
            var result = await _apiHelper.PostAsync<ResultModel>(url, new { custId, count });
            return Json(result);
        }
    }
}