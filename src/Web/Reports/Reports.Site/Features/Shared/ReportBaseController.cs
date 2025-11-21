using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Shared;

public abstract class ReportBaseController(IMediator mediator) : BaseController
{
    private const string InvalidViewName = "InvalidReport";
    protected readonly IMediator mediator = mediator;

    protected async Task<IActionResult> View<TResponse>(IRequest<TResponse> message, string viewName = null)
    {
        var baseQuery = message as BaseQuery;

        if (baseQuery == null || !baseQuery.IsValid())
        {
            return View(InvalidViewName);
        }

        try
        {
            return View(viewName, await mediator.Send(message));
        }
        catch (UnauthorizedAccessException)
        {
            return View(InvalidViewName);
        }
    }

    protected async Task<IActionResult> Json<TResponse>(
        IRequest<TResponse> message,
        JsonIgnoreCondition jsonIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)
    {
        var baseQuery = message as BaseQuery;
        if (baseQuery == null || !baseQuery.IsValid())
        {
            return Json(new Reports.Features.Shared.Models.Result((int)HttpStatusCode.Forbidden, Resources.NotAuthorized));
        }

        try
        {
            return Json(await mediator.Send(message), new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                DefaultIgnoreCondition = jsonIgnoreCondition
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Json(new Reports.Features.Shared.Models.Result((int)HttpStatusCode.Forbidden, Resources.NotAuthorized));
        }
    }

    protected async Task<IActionResult> ExcelView(IRequest<ExcelFileViewModel> message)
    {
        var baseQuery = message as BaseQuery;

        if (baseQuery == null || !baseQuery.IsValid())
        {
            return GetExcelContent(new ExcelFileViewModel(InvalidViewName, []));
        }

        try
        {
            return GetExcelContent(await mediator.Send(message));
        }
        catch (UnauthorizedAccessException)
        {
            return GetExcelContent(new ExcelFileViewModel(InvalidViewName, []));
        }
    }

    private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
        => File(viewModel.Content, viewModel.ContentType, viewModel.Name);
}
