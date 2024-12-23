using System.Net;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers;

public class RewardController(IMediator mediator) : TransferBaseController(mediator)
{
    #region Reward Payment
    public Task<IActionResult> Payment(RewardPaymentQuery message) => View(message);

    public Task<IActionResult> GetPayment(GetRewardPaymentDataQuery message) => View(message, "PaymentData");

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePaymentStatus(UpdateRewardPaymentStatusCommand message)
    {
        if (!message.IsValid())
        {
            return Json(new Result((int)HttpStatusCode.Forbidden, Resources.NotAuthorized));
        }

        var result = await mediator.Send(message);

        if (result.IsKickout)
        {
            await ClearSessionAndSignOut();
        }

        return Json(result);
    }

    public Task<FileContentResult> PaymentExcel(RewardPaymentExcelQuery message) => ExcelView(message);
    #endregion

    #region Reward Detail
    public Task<IActionResult> Detail(RewardDetailQuery message) => View(message);

    public Task<IActionResult> GetDetail(GetRewardDetailDataQuery message) => View(message, "DetailData");

    public Task<FileContentResult> DetailExcel(RewardDetailExcelQuery message) => ExcelView(message);
    #endregion
}