using AlphaPlus.Core.Filters;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class LegendsController(IMediator mediator) : ReportBaseController(mediator)
{
    [AllowProtectedMode]
    public async Task<IActionResult> Index(LegendQuery message)
        => View(await mediator.Send(message));

    [AllowProtectedMode]
    public async Task<IActionResult> WinLoss(WinLossLegendQuery message)
        => View(nameof(Index), await mediator.Send(message));

    [AllowProtectedMode]
    public async Task<IActionResult> WinLossDetail(WinLossDetailLegendQuery message)
        => View(nameof(Index), await mediator.Send(message));

    public async Task<IActionResult> MatchWinLossDetail(MatchWinLossDetailLegendQuery message)
        => View(nameof(Index), await mediator.Send(message));
}
