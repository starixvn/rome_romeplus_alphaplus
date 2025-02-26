using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Core;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;

public abstract class SiteBaseController(IMediator mediator) : BaseController
{
    private const string InvalidExcelFileName = "Invalid";
    protected const string InvalidPageViewPath = "~/Features/MemberInfo/Views/Shared/InvalidPage.cshtml";
    protected const int BadRequestErrorCode = 999;

    protected readonly IMediator mediator = mediator;

    protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message, string viewName)
        => (message as MemberInfoBaseQuery).IsValid()
            ? View(viewName, await mediator.Send(message))
            : View(InvalidPageViewPath);

    protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message)
        => (message as MemberInfoBaseQuery).IsValid()
            ? GetExcelContent(await mediator.Send(message))
            : GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, []));

    protected async Task<ActionResult> Json<TResponse>(IRequest<TResponse> message)
        => (message as MemberInfoBaseQuery).IsValid() ?
            Json(await mediator.Send(message)) :
            Json(Result.CreateErrorResult(Lang.NotAuthorized, BadRequestErrorCode));

    private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
        => File(viewModel.Content, viewModel.ContentType, viewModel.Name);

    protected async Task<ActionResult> ViewAsync<TResponse>(IRequest<TResponse> message)
        => (message as MemberInfoBaseQuery).IsValid()
            ? View(await mediator.Send(message))
            : View(InvalidPageViewPath);

    protected async Task<ActionResult> ViewAsync<TResponse>(IRequest<TResponse> message, string viewName)
        => (message as MemberInfoBaseQuery).IsValid()
            ? View(viewName, await mediator.Send(message))
            : View(InvalidPageViewPath);

    protected async Task<ActionResult> JsonAsync<TResponse>(
        IRequest<TResponse> message)
        => (message as MemberInfoBaseQuery).IsValid() ?
            Json(await mediator.Send(message)) :
            Json(Result.CreateErrorResult(Lang.NotAuthorized, BadRequestErrorCode));

    protected async Task<FileContentResult> ExcelViewAsync(IRequest<ExcelFileViewModel> message)
        => (message as MemberInfoBaseQuery).IsValid()
            ? GetExcelContent(await mediator.Send(message))
            : GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, []));
}
