using System.Net;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class BetListBaseController(IMediator mediator) : BaseController
{
    protected const string InvalidBetListView = "~/Features/Shared/Views/Invalid.cshtml";
    protected const string BetListView = "~/Features/BetLists/Views/{0}/{1}.cshtml";
    protected readonly IMediator mediator = mediator;

    protected async Task<IActionResult> View<TResponse>(IRequest<TResponse> message)
        => View(await mediator.Send(message));

    protected async Task<IActionResult> View<TResponse>(IRequest<TResponse> message, string viewName)
        => View(viewName, await mediator.Send(message));

    protected async Task<IActionResult> PartialView<TResponse>(IRequest<TResponse> message, string viewName)
        => PartialView(viewName, await mediator.Send(message));

    protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message)
    {
        var viewModel = await mediator.Send(message);

        return File(viewModel.Content, viewModel.ContentType, viewModel.Name);
    }

    // protected async Task<IActionResult> Json<TResponse>(IRequest<TResponse> message)
    //     => Json(await mediator.Send(message));

    // protected Result ForbiddenResult()
    //     => Result.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

    // protected Result BadRequestResult()
    //     => Result.CreateErrorResult(Resources.BadRequest, (int)HttpStatusCode.BadRequest);

    protected static string GetViewPath(string controllerName, string actionName)
        => $"~/Features/BetLists/Views/{controllerName}/{actionName}.cshtml";
}
