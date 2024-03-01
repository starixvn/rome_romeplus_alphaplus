using System.Net;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class BetListBaseController : BaseController
{
    protected const string InvalidBetListView = "~/Features/Views/InvalidBetList.cshtml";
    protected readonly IMediator mediator;

    protected BetListBaseController(IMediator mediator)
        => this.mediator = mediator;

    private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
        => File(viewModel.Content, viewModel.ContentType, viewModel.Name);

    protected async Task<IActionResult> View<TResponse>(IRequest<TResponse> message, CancellationToken cancellation)
        => View(await mediator.Send(message, CancellationToken cancellation));

    protected async Task<IActionResult> View<TResponse>(string viewName, IRequest<TResponse> message, CancellationToken cancellation)
        => View(viewName, await mediator.Send(message, CancellationToken cancellation));

    protected async Task<IActionResult> PartialView<TResponse>(IRequest<TResponse> message, CancellationToken cancellation)
        => PartialView(await mediator.Send(message, CancellationToken cancellation));

    protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message, CancellationToken cancellation)
        => GetExcelContent(await mediator.Send(message, CancellationToken cancellation));

    protected async Task<IActionResult> View<TResponse>(IRequest<TResponse> message, string viewName)
        => View(viewName, await mediator.Send(message, CancellationToken cancellation));

    protected async Task<IActionResult> Json<TResponse>(IRequest<TResponse> message, CancellationToken cancellation)
        => Json(await mediator.Send(message, CancellationToken cancellation));

    protected Result ForbiddenResult()
        => Result.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

    protected Result BadRequestResult()
        => Result.CreateErrorResult(Resources.BadRequest, (int)HttpStatusCode.BadRequest);
}
