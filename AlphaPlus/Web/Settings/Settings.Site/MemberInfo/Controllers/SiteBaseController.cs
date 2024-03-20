namespace AlphaPlus.Settings.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.Site.ViewModels;
    using AlphaPlus.Core;
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    public abstract class SiteBaseController : BaseController
    {
        protected const string InvalidPageViewPath = "~/MemberInfo/Views/Shared/InvalidPage.cshtml";
        private const string InvalidExcelFileName = "Invalid";
        protected const int BadRequestErrorCode = 999;

        protected readonly IMediator mediator;

        protected SiteBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message, string viewName)
            => (message as MemberInfoBaseQuery).IsValid()
                ? View(viewName, await mediator.Send(message))
                : View(InvalidPageViewPath);

        protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message)
            => (message as MemberInfoBaseQuery).IsValid()
                ? GetExcelContent(await mediator.Send(message))
                : GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, new byte[] { }));

        protected async Task<ActionResult> Json<TResponse>(IRequest<TResponse> message)
            => (message as MemberInfoBaseQuery).IsValid() ? 
                Json(await mediator.Send(message)) :
                Json(Result.CreateErrorResult(Lang.NotAuthorized, BadRequestErrorCode));

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
            => File(viewModel.Content, ExcelFileViewModel.ContentType, viewModel.Name);

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
                : GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, new byte[] { }));
    }
}