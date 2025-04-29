using AlphaPlus.CashService.Site.Features.Payments;
using AlphaPlus.CashService.Site.Features.Promotions.Models;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace AlphaPlus.CashService.Site.Features.Promotions
{
    [Route("[controller]")]
    public class PromotionsController : Controller
    {
        private readonly ILogger<PromotionsController> _logger;

        private readonly string _apiBaseUrl;
        private static HttpClient _httpClient;

        public PromotionsController(ILogger<PromotionsController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return Ok("Promotions");
        }

        public async Task<IActionResult> GetPromotion( CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/promotion/get-promotion";
            
            var result  = await _httpClient.GetFromJsonAsync<IEnumerable<PromotionModel>>(url, cancellationToken);
          
            return Json(result);
        }
    }
}
