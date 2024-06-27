using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings.ProductResources;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.Shared
{
    public class SiteBaseController : BaseController
    {
        protected const string InvalidPageViewPath = "~/Features/MemberInfo/Views/Shared/InvalidPage.cshtml";
        private const string InvalidExcelFileName = "Invalid";
        protected const int BadRequestErrorCode = 999;

        protected readonly IMediator mediator;

        protected SiteBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message)
            => (message as BaseQuery).IsValid()
                ? View(await mediator.Send(message))
                : View(InvalidPageViewPath);

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message, string viewName)
            => (message as BaseQuery).IsValid()
                ? View(viewName, await mediator.Send(message))
                : View(InvalidPageViewPath);


        protected async Task<ActionResult> Json<TResponse>(IRequest<TResponse> message)
            => (message as BaseQuery).IsValid() ?
                Json(await mediator.Send(message)) :
                Json(Result.CreateErrorResult(ProductResources.NotAuthorized, BadRequestErrorCode));
    }
}