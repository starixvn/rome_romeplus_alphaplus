namespace AlphaPlus.Features.Site.Messages.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MediatR;
    using AlphaPlus.Core;

    public abstract class SiteBaseController : BaseController
    {
        protected readonly IMediator mediator;

        protected SiteBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected ActionResult View<TResponse>(IRequest<TResponse> message)
            => View(mediator.Send(message));

        protected async Task<ActionResult> Json<TResponse>(IRequest<TResponse> message)
            => new JsonResult(await mediator.Send(message));

        protected async Task<ActionResult> JsonAsync<TResponse>(
            IRequest<TResponse> message)
            => Json(await mediator.Send(message));
    }
}