using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers;

public class TransferYesterdayBalanceController(IMediator mediator) : TransferBaseController(mediator)
{
    public async Task<IActionResult> Single(SingleQuery message)
    {
        if (!message.IsValid())
        {
            return View(InvalidPageViewPath);
        }

        var viewModel = await mediator.Send(message);

        if (viewModel == null)
        {
            return View(InvalidPageViewPath);
        }

        return View(viewModel);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Single(SingleCommand message)
        => Json(!message.IsValid() || message.IsInternal
        ? Result.CreateErrorResult(Resources.NotAuthorized)
        : await mediator.Send(message));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Multiple(MultipleCommand message)
        => Json(!message.IsValid() || message.IsInternal
        ? Result.CreateErrorResult(Resources.NotAuthorized)
        : await mediator.Send(message));

    public Task<IActionResult> Member(MemberQuery message) => View(message);

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Member(MemberCommand message)
        => Json(!message.IsValid() || message.IsInternal
        ? Result.CreateErrorResult(Resources.NotAuthorized)
        : await mediator.Send(message));
}
