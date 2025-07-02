using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers;

public class CashBalanceTransferController(IMediator mediator) : TransferBaseController(mediator)
{
    [HttpGet]
    public Task<IActionResult> CashBalance(CashBalanceTransferQuery message) => View(message);

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CashBalance(CashBalanceTransferCommand command)
    {
        if (command.IsInternal)
        {
            return Json(Result.CreateErrorResult(Resources.NotAuthorized, 403));
        }

        return await Json(command);
    }

    [HttpGet]
    public Task<IActionResult> Hint(HintTransferQuery message)
        => View(message, GetHintViewNameByLanguage(message.Language));

    [NonAction]
    private string GetHintViewNameByLanguage(string language)
        => language switch
        {
            "zh-TW" or "zh-CN" or "th-TH" or "vi-VN" or "ko-KR" => $"{nameof(Hint)}.{language}",
            _ => nameof(Hint)
        };
}