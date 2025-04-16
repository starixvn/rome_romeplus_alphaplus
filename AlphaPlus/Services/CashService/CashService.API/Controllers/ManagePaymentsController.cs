using AlphaPlus.CashService.API.Models.ManagePayments;
using AlphaPlus.CashService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagePaymentsController : ControllerBase
    {
        private readonly IManagePaymentsService _managePaymentsService;

        public ManagePaymentsController(IManagePaymentsService managePaymentsService)
        {
            _managePaymentsService = managePaymentsService;
        }

        [HttpGet("providers")]
        public async Task<ActionResult<IEnumerable<PaymentProviderModel>>> GetAllProviders()
        {
            var providers = await _managePaymentsService.GetAllPaymentProviders();
            return Ok(providers);
        }
    }
} 