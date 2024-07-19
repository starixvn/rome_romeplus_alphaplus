using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Core;
using AlphaPlus.Settings.Site.Features.Transfer.Requests;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels;

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
}