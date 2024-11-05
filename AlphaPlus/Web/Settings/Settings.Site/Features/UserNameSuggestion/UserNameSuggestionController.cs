using AlphaPlus.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.UserNameSuggestion
{
    public class UserNameSuggestionController(IMediator mediator) : BaseController
    {
        private readonly IMediator mediator = mediator;

        [HttpGet]
        public async Task<ActionResult> Index(UserNameSuggestionQuery message)
            => Json(await mediator.Send(message));

        [HttpGet]
        public async Task<ActionResult> GetUsernamesSuggestionByRole(UserNameSuggestionByRoleQuery message)
            => Json(await mediator.Send(message));
    }
}