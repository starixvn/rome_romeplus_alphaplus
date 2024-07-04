using AlphaPlus.Core;
using AlphaPlus.Core.Models;
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
}