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

        public PaymentsController(ILogger<PaymentsController> logger, IWebHostEnvironment environment, APIHelper apiHelper, IConfiguration configuration)
        {
            _logger = logger;
            _environment = environment;
            _apiHelper = apiHelper;
            _apiBaseUrl = configuration["CashServiceAPIUrl"];
        }

        public IActionResult Index()
        {
            return View();
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

        public async Task<IActionResult> GetAllProviders()
        {
            var url = $"{_apiBaseUrl}/payment-providers";
            var providers = await _apiHelper.GetAsync<List<ProviderModel>>(url);
            return Json(providers);
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