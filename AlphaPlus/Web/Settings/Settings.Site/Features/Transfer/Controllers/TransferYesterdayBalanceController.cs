using Microsoft.AspNetCore.Mvc;
using MediatR;
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
    public Task<IActionResult> Single(SingleCommand message)
        => Json(message);

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Multiple(MultipleCommand message)
        => Json(message);

    public Task<IActionResult> Member(MemberQuery message) => View(message);

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Member(MemberCommand message)
        => Json(message);

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> MultipleMember(MultipleMemberCommand message)
        => Json(message);
}
