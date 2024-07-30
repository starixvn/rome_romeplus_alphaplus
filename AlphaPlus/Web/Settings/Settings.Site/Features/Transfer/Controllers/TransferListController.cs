using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferList;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers;

public class TransferListController(IMediator mediator) : TransferBaseController(mediator)
{
    public Task<IActionResult> Index(TransferListQuery message) => View(message);
}
