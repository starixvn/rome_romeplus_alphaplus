// using Microsoft.AspNetCore.Mvc;
// using MediatR;
// using AlphaPlus.Settings.Site.Features.Transfer.Requests.MultipleTransfer;
// using AlphaPlus.Core.Models;
// using AlphaPlus.Shared.AppResources.Settings.Transfer;

// namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers;

// public class MultipleTransferController(IMediator mediator) : TransferBaseController(mediator)
// {
//     public Task<IActionResult> Index(TransferListQuery message) => View(message);

//     public Task<IActionResult> Single(SingleTransferQuery message)
//         => View(message);

//     [HttpPost, ValidateAntiForgeryToken]
//     public async Task<IActionResult> Single(SingleTransferCommand message)
//         => Json(message.IsInternal
//         ? Result.CreateErrorResult(Resources.NotAuthorized)
//         : await mediator.Send(message));

//     [HttpPost, ValidateAntiForgeryToken]
//     public async Task<IActionResult> Multiple(MultipleTransferCommand message)
//         => Json(message.IsInternal
//         ? Result.CreateErrorResult(Resources.NotAuthorized)
//         : await mediator.Send(message));
// }
