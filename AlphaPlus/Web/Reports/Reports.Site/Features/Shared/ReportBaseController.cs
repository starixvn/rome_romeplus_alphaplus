using System.Net;
using AlphaPlus.Core;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Shared;

public abstract class ReportBaseController(IMediator mediator) : BaseController
{
    private const string InvalidViewName = "InvalidReport";
    protected readonly IMediator mediator = mediator;

    protected async Task<IActionResult> View<TResponse>(IRequest<TResponse> message, string viewName = null)
    {
        var baseQuery = message as BaseQuery;

        return baseQuery != null && baseQuery.IsValid()
            ? View(viewName, await mediator.Send(message))
            : View(InvalidViewName);
    }

    protected async Task<IActionResult> Json<TResponse>(IRequest<TResponse> message)
    {
        var baseQuery = message as BaseQuery;
        if (baseQuery != null && baseQuery.IsValid())
        {
            return Json(await mediator.Send(message));
        }

        return Json(new Result((int)HttpStatusCode.Forbidden, Resources.NotAuthorized));
    }

    protected async Task<IActionResult> ExcelView(IRequest<ExcelFileViewModel> message)
    {
        var baseQuery = message as BaseQuery;

        return baseQuery != null && baseQuery.IsValid()
            ? GetExcelContent(await mediator.Send(message))
            : GetExcelContent(new ExcelFileViewModel(InvalidViewName, []));
    }

    private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
        => File(viewModel.Content, viewModel.ContentType, viewModel.Name);
}
