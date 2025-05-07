using AlphaPlus.CashService.Site.Features.Promotions.Models;
using AlphaPlus.CashService.Site.Features.Promotions.ViewModels;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Promotions
{
    public class PromotionsController : Controller
    {
        private readonly ILogger<PromotionsController> _logger;
        private readonly string _apiBaseUrl;
        private static HttpClient _httpClient;
        private readonly IUserSession _userSession;

        public PromotionsController(
            ILogger<PromotionsController> logger,
            HttpClient httpClient,
            IConfiguration configuration,
            IUserSession userSession)
        {
            _logger = logger;
            _apiBaseUrl = configuration["CashServiceAPIUrl"];
            _httpClient = httpClient;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            CancellationToken cancellationToken = CancellationToken.None;

            var url = $"{_apiBaseUrl}/promotion/get-promotion";
            var promotion = await _httpClient.GetFromJsonAsync<IEnumerable<PromotionModel>>(url, cancellationToken);

            var viewModel = new PromotionViewModel
            {
                Language = _userSession.Language,
                DarkModeName = _userSession.DarkModeName,
                Promotion = promotion
            };

            return View(viewModel);
        }

        public async Task<IActionResult> GetPromotion(CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/promotion/get-promotion";

            var result = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);

            return Json(result);
        }
    }
}
