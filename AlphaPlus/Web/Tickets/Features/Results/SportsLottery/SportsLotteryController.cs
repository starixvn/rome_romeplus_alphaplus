using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.SportsLottery;

public partial class ResultsController(IMediator mediator) : ResultBaseController(mediator)
{
    public Task<IActionResult> SportsLottery(SportsLotteryResultQuery message)
        => View(message, string.Format(ViewPath, nameof(SportsLottery), "Index"));
}