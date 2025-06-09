using AlphaPlus.CashService.Site.Features.Promotions.ViewModels;
using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.CashService.Site.Features.Shared.Filters;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Promotions
{
    [SiteAuthorizationFilter(PageAccessType.CashPromotion)]
    public class PromotionsController : SiteBaseController
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;

        public PromotionsController(
            HttpClient httpClient,
            IConfiguration configuration,
            IUserSession userSession,
            IAntiforgery antiforgery) : base(userSession, antiforgery)
        {
            _apiBaseUrl = configuration["CashService:APIUrl"];
            _httpClient = httpClient;
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
