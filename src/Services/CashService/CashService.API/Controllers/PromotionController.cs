using AlphaPlus.CashService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromotionController(IPromotionService service) : ControllerBase
{
    [Route("get-promotion")]
   public async Task<IActionResult> GetPromotion(string type, string status)
    {
        var result = await service.GetPromotion(type, status);
        
        return Ok(result);
    }

    [Route("add-promotion")]
    public async Task<IActionResult> AddPromotion()
    {
        var result = string.Empty;

        return Ok(result);
    }
}
