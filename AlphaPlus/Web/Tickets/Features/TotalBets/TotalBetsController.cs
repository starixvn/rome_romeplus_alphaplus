using AlphaPlus.BetsForecast.Domain.TotalBets;
using AlphaPlus.Core;
using AlphaPlus.Tickets.Site.Features.TotalBets.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.TotalBets;

public class TotalBetsController : BaseController
{
    private const string PartialName = "Partial";
    private readonly ITotalBetsService totalBetsService;

    public TotalBetsController(ITotalBetsService totalBetsService)
        => this.totalBetsService = totalBetsService;

    public Task<IActionResult> HDPOULive(GetTotalBetsRequest message, CancellationToken cancellation)
        => TotalBetsView<HDPOULive>(nameof(HDPOULive), message, CancellationToken cancellation);

    public Task<IActionResult> MoneyLine(GetTotalBetsRequest message, CancellationToken cancellation)
        => TotalBetsView<MoneyLine>(nameof(MoneyLine), message, CancellationToken cancellation);

    public Task<IActionResult> OE1X2DND(GetTotalBetsRequest message, CancellationToken cancellation)
        => TotalBetsView<OE1X2DND>(nameof(OE1X2DND), message, CancellationToken cancellation);

    public Task<IActionResult> TotalGoal(GetTotalBetsRequest message, CancellationToken cancellation)
        => TotalBetsView<TotalGoal>(nameof(TotalGoal), message, CancellationToken cancellation);

    public Task<IActionResult> Outright(GetTotalBetsRequest message, CancellationToken cancellation)
        => TotalBetsView<Outright>(nameof(Outright), message, CancellationToken cancellation);

    public Task<IActionResult> HTFT(GetTotalBetsRequest message, CancellationToken cancellation)
        => TotalBetsView<HalfTimeFullTime>(nameof(HTFT), message, CancellationToken cancellation);

    public Task<IActionResult> FGLG(GetTotalBetsRequest message, CancellationToken cancellation)
       => TotalBetsView<FirstGoalLastGoal>(nameof(FGLG), message, CancellationToken cancellation);

    public Task<IActionResult> HomeDrawAwayNoBet(GetTotalBetsRequest message, CancellationToken cancellation)
       => TotalBetsView<HomeDrawAwayNoBet>(nameof(HomeDrawAwayNoBet), message, CancellationToken cancellation);

    public Task<IActionResult> NumberGame(GetTotalBetsRequest message, CancellationToken cancellation)
       => TotalBetsView<NumberGame>(nameof(NumberGame), message, CancellationToken cancellation);

    private async Task<IActionResult> TotalBetsView<T>(string action, GetTotalBetsRequest message, CancellationToken cancellation)
    {
        var totalBets = await totalBetsService.GetTotalBets<T>(
            action,
            message.ViewerLevel,
            message.ViewerId,
            message.Language,
            message.SiteName);

        return View($"{action}{(message.Partial ? PartialName : string.Empty)}", totalBets);
    }
}