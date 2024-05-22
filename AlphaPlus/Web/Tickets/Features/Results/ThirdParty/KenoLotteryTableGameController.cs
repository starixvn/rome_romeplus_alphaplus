using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public partial class ResultsController : ResultBaseController
{
    private const string KenoLotteryTableGameView = "~/Features/Results/ThirdParty/Views/KenoLotteryTableGame.cshtml";

    public Task<IActionResult> Keno(KenoResultQuery message)
        => View(message, KenoLotteryTableGameView);

    public Task<IActionResult> Lottery(LotteryResultQuery message)
        => View(message, KenoLotteryTableGameView);

    public Task<IActionResult> TableGames(TableGamesResultQuery message)
        => View(message, KenoLotteryTableGameView);

    public Task<IActionResult> SportsLottery(SportsLotteryResultQuery message)
        => View(message, KenoLotteryTableGameView);

    public Task<IActionResult> VirtualGames(VirtualGamesResultQuery message)
        => View(message, KenoLotteryTableGameView);
}