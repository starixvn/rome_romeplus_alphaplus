using AlphaPlus.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.UserNameSuggestion;

public class UserNameSuggestionController(IMediator mediator) : BaseController
{
    private readonly IMediator mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Index(UserNameSuggestionQuery message)
        => Json(message.IsValid() ? await mediator.Send(message) : []);

    [HttpGet]
    public async Task<IActionResult> GetUsernamesSuggestionByRole(UserNameSuggestionByRoleQuery message)
        => Json(message.IsValid() ? await mediator.Send(message) : []);
}