namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Shared.AppResources.Settings.Transfer;
    using AlphaPlus.Settings.Transfer.API.Services;
    using MediatR;
    using Requests.TransferToDownlineHistory;
    using AlphaPlus.Core.Models;

    public class TransferToDownlineHistoryController : SiteBaseController
    {
        private const string InvalidTransferStatusViewPath = "~/Features/Transfer/Views/Shared/InvalidTransferStatus.cshtml";
        private readonly TransferService transferService;

        public TransferToDownlineHistoryController(
            IMediator mediator,
            TransferService transferService) : base(mediator)
        {
            this.transferService = transferService;
        }

        public async Task<IActionResult> Index(TransferToDownlineHistoryQuery message)
        {
            if (message != null && transferService.IsAllDayBalanceTransfer(message.ViewerId))
            {
                return await View(message);
            }

            return View(InvalidTransferStatusViewPath);
        }

        public async Task<ActionResult> Excel(TransferToDownlineHistoryExcelQuery message)
        {
            if (message != null && transferService.IsAllDayBalanceTransfer(message.ViewerId))
            {
                return await ExcelView(message);
            }

            return View(InvalidTransferStatusViewPath);
        }

        public async Task<JsonResult> GetTransferToDownlineHistoryData(TransferToDownlineHistoryPagingQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(message.ViewerId))
            {
                return Json(await mediator.Send(message));
            }

            return Json(Result.CreateErrorResult(Resources.DisallowedAllDayBalanceTransferError, 6001));
        }
    }
}