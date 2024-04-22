namespace AlphaPlus.Settings.Site.Transfer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Settings.Transfer.API.AppResources;
    using AlphaPlus.Settings.Transfer.API.Models;
    using AlphaPlus.Settings.Transfer.API.Services;
    using MediatR;
    using Requests.TransferToDownlineHistory;

    public class TransferToDownlineHistoryController : SiteBaseController
    {
        private const string InvalidTransferStatusViewPath = "~/Transfer/Views/Shared/InvalidTransferStatus.cshtml";
        private readonly TransferService transferService;

        public TransferToDownlineHistoryController(
            IMediator mediator,
            TransferService transferService) : base(mediator)
        {
            this.transferService = transferService;
        }

        public async Task<ActionResult> Index(TransferToDownlineHistoryQuery message)
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