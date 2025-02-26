using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferCondition;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers;

public class TransferConditionController(IMediator mediator) : TransferBaseController(mediator)
{
    public Task<IActionResult> Index(TransferConditionQuery message) => View(message);

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Update(UpdateTransferConditionCommand message) => Json(message);
}
