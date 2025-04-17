using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Payments
{
    public class PaymentsController : Controller
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IWebHostEnvironment _environment;

        public PaymentsController(ILogger<PaymentsController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}