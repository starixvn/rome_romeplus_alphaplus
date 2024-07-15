using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.MultipleTransfer;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers;

public class MultipleTransferController(IMediator mediator) : TransferBaseController(mediator)
{
    public Task<IActionResult> Index(TransferListQuery message) => View(message);
}
