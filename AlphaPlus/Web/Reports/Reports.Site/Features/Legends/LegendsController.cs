using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class LegendsController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(LegendQuery message) => View(message);

    public Task<IActionResult> WinLoss(WinLossLegendQuery message)
        => View(message, nameof(Index));

    public Task<IActionResult> MatchWinLossDetail(MatchWinLossDetailLegendQuery message)
        => View(message, nameof(Index));
}
