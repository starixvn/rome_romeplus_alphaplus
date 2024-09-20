using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results;

public abstract class ResultBaseController(IMediator mediator) : BaseController
{
    private const string ViewPath = "~/Features/Results/{0}/Views/{1}.cshtml";
    protected readonly IMediator mediator = mediator;

    protected async Task<IActionResult> View<TResponse>(IRequest<TResponse> message, string viewName)
        => View(viewName, await mediator.Send(message));

    protected async Task<IActionResult> ExcelView(IRequest<ExcelFileViewModel> message)
    {
        var viewModel = await mediator.Send(message);

        return File(viewModel.Content, viewModel.ContentType, viewModel.Name);
    }

    protected static string GetViewPath(string controllerName, string actionName)
        => string.Format(ViewPath, controllerName, actionName);
}