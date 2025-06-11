using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Alpha.Handbook.Demo.Controllers
{
    public class BaseController : Controller
    {
        public readonly IMediator mediator;

        public BaseController(IMediator mediator)
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