using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Core;

namespace AlphaPlus.Features.Site.Messages.Controllers
{
    public abstract class SiteBaseController : BaseController
    {
        protected readonly IMediator mediator;

        protected SiteBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message)
            => View(await mediator.Send(message));

        protected async Task<ActionResult> Json<TResponse>(IRequest<TResponse> message)
            => new JsonResult(await mediator.Send(message));

        protected async Task<ActionResult> JsonAsync<TResponse>(
            IRequest<TResponse> message)
            => Json(await mediator.Send(message));
    }
}