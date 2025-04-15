using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Transactions
{
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ILogger<TransactionsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Ok("Transactions");
        }
    }
}