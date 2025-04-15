using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MediatR;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Transfer.API.Services;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Configurations;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers;

public class TransferToDownlineHistoryController(
    IMediator mediator,
    ITransferService transferService) : TransferBaseController(mediator)
{
    private const string InvalidTransferStatusViewPath = "~/Features/Transfer/Views/Shared/InvalidTransferStatus.cshtml";
    private readonly ITransferService transferService = transferService;

    public async Task<IActionResult> Index(TransferToDownlineHistoryQuery message)
    {
        if (await transferService.IsAllDayBalanceTransfer(message.ViewerId))
        {
            return View(new TransferToDownlineHistoryViewModel(message));
        }

        return View(InvalidTransferStatusViewPath);
    }

    public async Task<IActionResult> Excel(TransferToDownlineHistoryExcelQuery message)
    {
        if (await transferService.IsAllDayBalanceTransfer(message.ViewerId))
        {
            return await ExcelView(message);
        }

        return View(InvalidTransferStatusViewPath);
    }

    public async Task<JsonResult> GetTransferToDownlineHistoryData(TransferToDownlineHistoryPagingQuery message)
    {
        if (await transferService.IsAllDayBalanceTransfer(message.ViewerId))
        {
            return Json(await mediator.Send(message));
        }

        return Json(Result.CreateErrorResult(Resources.DisallowedAllDayBalanceTransferError, 6001));
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (!AppSettings.IsAllowedTransferToDownlineHistorySite(UserSession.Current.Site))
        {
            context.Result = View("~/Features/Transfer/Views/Shared/InvalidPage.cshtml");
        }
    }
}