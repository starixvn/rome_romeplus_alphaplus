using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Core;
using AlphaPlus.Settings.Site.Features.Transfer.Requests;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Authentication;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers;

public abstract class TransferBaseController(IMediator mediator) : BaseController
{
    protected readonly IMediator mediator = mediator;

    protected const string InvalidPageViewPath = "~/Features/Transfer/Views/Shared/InvalidPage.cshtml";
    protected const string InvalidExcelFileName = "InvalidExcelFile";

    protected async Task<IActionResult> View<TResponse>(IRequest<TResponse> message)
    {
        if (message is TransferBaseQuery transferMessage && !transferMessage.IsValid())
        {
            return View(InvalidPageViewPath);
        }

        return View(await mediator.Send(message));
    }

    protected async Task<IActionResult> View<TResponse>(IRequest<TResponse> message, string viewName)
    {
        if (message is TransferBaseQuery transferMessage && !transferMessage.IsValid())
        {
            return View(InvalidPageViewPath);
        }

        return View(viewName, await mediator.Send(message));
    }

    protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message)
    {
        if (message is TransferBaseQuery transferMessage && !transferMessage.IsValid())
        {
            return GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, []));
        }

        return GetExcelContent(await mediator.Send(message));
    }

    private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
        => File(viewModel.Content, viewModel.ContentType, viewModel.Name);

    protected async Task<IActionResult> Json<TResponse>(IRequest<TResponse> message)
    {
        if (message is TransferBaseQuery transferMessage && !transferMessage.IsValid())
        {
            return Json(Result.CreateErrorResult(Resources.NotAuthorized));
        }

        return Json(await mediator.Send(message));
    }

    protected async Task ClearSessionAndSignOut()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync();
    }
}