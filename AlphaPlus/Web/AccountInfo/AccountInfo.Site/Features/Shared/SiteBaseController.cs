using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.AccountInfo.Site.Features.Shared
{
    public class SiteBaseController : BaseController
    {
        private const string NoInformationPageViewPath = "~/Features/Shared/Views/NoInformation.cshtml";
        protected readonly IMediator mediator;

        protected SiteBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message)
            => (message as BaseQuery).IsValid()
                ? View(await mediator.Send(message))
                : View(NoInformationPageViewPath);

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message, string viewName)
            => (message as BaseQuery).IsValid()
                ? View(viewName, await mediator.Send(message))
                : View(NoInformationPageViewPath);


        protected async Task<ActionResult> Json<TResponse>(IRequest<TResponse> message)
            => (message as BaseQuery).IsValid() ?
                Json(await mediator.Send(message)) :
                Json(Result.CreateErrorResult("You are not authorized on this function.", StatusCodes.Status403Forbidden));
    }
}