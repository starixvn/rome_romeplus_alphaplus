using AlphaPlus.CashService.Site.Features.Promotions.ViewModels;
using AlphaPlus.CashService.Site.Features.Shared.Filters;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Promotions
{
    [SiteAuthorizationFilter]
    public class PromotionsController : Controller
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;
        private readonly IUserSession _userSession;

        public PromotionsController(
            HttpClient httpClient,
            IConfiguration configuration,
            IUserSession userSession)
        {
            _apiBaseUrl = configuration["CashService:APIUrl"];
            _httpClient = httpClient;
            _userSession = userSession;
        }

        public IActionResult Index()
        {
            var viewModel = new PromotionBaseViewModel
            {
                Language = _userSession.Language,
                DarkModeName = _userSession.DarkModeName,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> GetPromotion(string promotionType, string status, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/promotion/get-promotion/{promotionType}/{status}";

            var result = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);

            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePromotion(string id, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/promotion/delete/{_userSession.CustId}/{id}";
            var response = await _httpClient.DeleteAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<object>(cancellationToken: cancellationToken);
            return Json(result);
        }
    }
}
