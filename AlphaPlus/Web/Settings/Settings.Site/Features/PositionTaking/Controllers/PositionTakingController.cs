using AlphaPlus.Core;
using AlphaPlus.Settings.Site.Features.PositionTaking.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.PositionTaking.Controllers;

public class PositionTakingController : BaseController
{
    private readonly IMediator mediator;

    public PositionTakingController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<IActionResult> Index(PositionTakingRequest request)
    {
        return View(await mediator.Send(request));
    }
}