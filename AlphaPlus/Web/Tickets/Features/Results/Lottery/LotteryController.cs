using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.Lottery;

public partial class ResultsController(IMediator mediator) : ResultBaseController(mediator)
{
    public Task<IActionResult> Lottery(LotteryResultQuery message)
        => View(message, string.Format(ViewPath, nameof(Lottery), "Index"));
}