namespace Alpha.MemberInfo.Site.Features.Shared
{
    using Alpha.MemberInfo.Site.Features.Shared.Requests;
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
        public ActionResult GetUsernamesSuggestion(UserNameSuggestionQuery message)
            => Json(mediator.Send(message));
    }
}