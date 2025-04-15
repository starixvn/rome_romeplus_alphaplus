using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Promotions
{
    [Route("[controller]")]
    public class PromotionsController : Controller
    {
        private readonly ILogger<PromotionsController> _logger;

        public PromotionsController(ILogger<PromotionsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Ok("Promotions");
        }
    }
}
