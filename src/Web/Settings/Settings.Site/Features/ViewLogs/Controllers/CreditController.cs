using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.ViewLogs.Helpers;
using MediatR;
using AlphaPlus.Settings.Site.Features.ViewLogs.Requests.Credit;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Controllers
{
    public class CreditController : ViewLogBaseController
    {
        private readonly IMediator mediator;

        public CreditController(
            IUserSession userSession,
            IMediator mediator,
            MicroSites microSites) : base(userSession, microSites)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Index(IndexQuery indexQuery)
        {
            var viewModel = await mediator.Send(indexQuery);

            return View(viewModel);
        }

        public async Task<ActionResult> Excel(IndexQuery indexQuery)
        {
            var excelQuery = new CreditLogExcelQuery(
                UserSession,
                null,
                indexQuery.IsShowOldValue,
                indexQuery.SportBetTypeVisibilities,
                indexQuery.ResourcesByBetTypeCacheRef,
                indexQuery.ResourcesByProductCacheRef)
            {
                FromDate = indexQuery.FromDate,
                ToDate = indexQuery.ToDate,
                Username = indexQuery.Username,
                ActionTypeId = indexQuery.ActionTypeId,
                PageIndex = indexQuery.PageIndex,
                PageSize = indexQuery.PageSize
            };

            var excelViewModel = await mediator.Send(excelQuery);

            return File(excelViewModel.Content, excelViewModel.ContentType, excelViewModel.Name);
        }
    }
}