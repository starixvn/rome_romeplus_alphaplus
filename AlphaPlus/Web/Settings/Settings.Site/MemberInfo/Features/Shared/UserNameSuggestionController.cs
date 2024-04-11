namespace AlphaPlus.Settings.Site.Features.Shared
{
    using AlphaPlus.Settings.Site.Features.Shared.Requests;
    using AlphaPlus.Core;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class UserNameSuggestionController : BaseController
    {
        private readonly IMediator mediator;

        public UserNameSuggestionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(UserNameSuggestionQuery message)
            => Json(await mediator.Send(message));
    }
}