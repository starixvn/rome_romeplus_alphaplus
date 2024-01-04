using AlphaPlus.Tickets.Site.Features.TotalBets.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.TotalBets;

public class TotalBetsController : Controller
{
    private const string PartialName = "Partial";
    private readonly ITotalBetsService totalBetsService;

    public TotalBetsController(ITotalBetsService totalBetsService)
        => this.totalBetsService = totalBetsService;

    public async Task<IActionResult> HDPOULive(GetTotalBetsRequest message)
        => TotalBetsView(
        nameof(HDPOULive),
        message.Partial,
        await totalBetsService.GetHDPOULive(message.ViewerLevel, message.ViewerId));

    public async Task<IActionResult> MoneyLine(GetTotalBetsRequest message)
        => TotalBetsView(
        nameof(MoneyLine),
        message.Partial,
        await totalBetsService.GetMoneyLine(message.ViewerLevel, message.ViewerId, message.Language));

    public async Task<IActionResult> OE1x2DND(GetTotalBetsRequest message)
        => TotalBetsView(
        nameof(OE1x2DND),
        message.Partial,
        await totalBetsService.GetOE1x2DND(message.ViewerLevel, message.ViewerId, message.Language));

    public async Task<IActionResult> TotalGoal(GetTotalBetsRequest message)
        => TotalBetsView(
        nameof(TotalGoal),
        message.Partial,
        await totalBetsService.GetTotalGoal(message.ViewerLevel, message.ViewerId, message.Language));

    public async Task<IActionResult> Outright(GetTotalBetsRequest message)
        => TotalBetsView(
        nameof(Outright),
        message.Partial,
        await totalBetsService.GetOutright(message.ViewerLevel, message.ViewerId, message.Language));

    public async Task<IActionResult> HTFT(GetTotalBetsRequest message)
        => TotalBetsView(
        nameof(HTFT),
        message.Partial,
        await totalBetsService.GetHalfTimeFullTime(message.ViewerLevel, message.ViewerId, message.Language));

    public async Task<IActionResult> FGLG(GetTotalBetsRequest message)
       => TotalBetsView(
       nameof(FGLG),
       message.Partial,
       await totalBetsService.GetFirstGoalLastGoal(message.ViewerLevel, message.ViewerId, message.Language));

    public async Task<IActionResult> HomeDrawAwayNoBet(GetTotalBetsRequest message)
       => TotalBetsView(
       nameof(HomeDrawAwayNoBet),
       message.Partial,
       await totalBetsService.GetHomeDrawAwayNoBet(message.ViewerLevel, message.ViewerId, message.Language));

    public async Task<IActionResult> NumberGame(GetTotalBetsRequest message)
       => TotalBetsView(
       nameof(NumberGame),
       message.Partial,
       await totalBetsService.GetNumberGame(message.ViewerLevel, message.ViewerId, message.Language));

    private IActionResult TotalBetsView(string viewName, bool partial, object data)
        => View($"{viewName}{(partial ? PartialName : string.Empty)}", data);
}