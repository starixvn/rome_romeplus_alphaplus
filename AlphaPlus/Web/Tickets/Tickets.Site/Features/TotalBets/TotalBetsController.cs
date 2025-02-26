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

    public Task<IActionResult> HDPOULive(GetTotalBetsRequest message)
        => TotalBetsView<HDPOULive>(nameof(HDPOULive), message);

    public Task<IActionResult> MoneyLine(GetTotalBetsRequest message)
        => TotalBetsView<MoneyLine>(nameof(MoneyLine), message);

    public Task<IActionResult> OE1X2DND(GetTotalBetsRequest message)
        => TotalBetsView<OE1X2DND>(nameof(OE1X2DND), message);

    public Task<IActionResult> TotalGoal(GetTotalBetsRequest message)
        => TotalBetsView<TotalGoal>(nameof(TotalGoal), message);

    public Task<IActionResult> Outright(GetTotalBetsRequest message)
        => TotalBetsView<Outright>(nameof(Outright), message);

    public Task<IActionResult> HTFT(GetTotalBetsRequest message)
        => TotalBetsView<HalfTimeFullTime>(nameof(HTFT), message);

    public Task<IActionResult> FGLG(GetTotalBetsRequest message)
       => TotalBetsView<FirstGoalLastGoal>(nameof(FGLG), message);

    public Task<IActionResult> HomeDrawAwayNoBet(GetTotalBetsRequest message)
       => TotalBetsView<HomeDrawAwayNoBet>(nameof(HomeDrawAwayNoBet), message);

    public Task<IActionResult> NumberGame(GetTotalBetsRequest message)
       => TotalBetsView<NumberGame>(nameof(NumberGame), message);

    private async Task<IActionResult> TotalBetsView<T>(string action, GetTotalBetsRequest message)
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