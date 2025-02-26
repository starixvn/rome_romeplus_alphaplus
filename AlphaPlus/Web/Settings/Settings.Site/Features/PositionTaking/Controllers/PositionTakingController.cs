using AlphaPlus.Core;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Settings.Site.Features.PositionTaking.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.PositionTaking.Controllers;

public class PositionTakingController(IMediator mediator, IProductService productService) : BaseController
{
    private readonly IMediator mediator = mediator;
    private readonly IProductService productService = productService;

    [HttpGet]
    public async Task<IActionResult> Index(PositionTakingRequest request)
    {
        var pt = await mediator.Send(request);
        var product = await productService.GetPTProducts(request.ActorId, AppSettings.SettingStatus);

        return View(new { pt, product });
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromBody]DownlinePositionTakingRequest request)
    {
        return Json(await mediator.Send(request));
    }
}