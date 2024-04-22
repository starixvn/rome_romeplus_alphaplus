namespace AlphaPlus.Settings.Site.Transfer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MediatR;
    using Requests;
    using ViewModels;
    using AlphaPlus.Core;

    public abstract class SiteBaseController : BaseController
    {
        protected readonly IMediator mediator;

        protected const string InvalidPageViewPath = "~/Transfer/Views/Shared/InvalidPage.cshtml";
        protected const string InvalidExcelFileName = "InvalidExcelFile";

        protected SiteBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message)
        {
            var query = message as TransferBaseQuery;
            if (query == null || !query.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View( await mediator.Send(message));
        }

        protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message)
        {
            var query = message as TransferBaseQuery;
            if (query == null || !query.IsValid())
            {
                return GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, new byte[] { }));
            }

            return GetExcelContent(await mediator.Send(message));
        }

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
            => File(viewModel.Content, viewModel.ContentType, viewModel.Name);
    }
}