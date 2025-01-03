using AlphaPlus.Core.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public partial class ResultsController(IMediator mediator) : ResultBaseController(mediator)
{
    private const string KenoLotteryTableGameView = "~/Features/Results/ThirdParty/Views/KenoLotteryTableGame.cshtml";

    [AllowProtectedMode]
    public Task<IActionResult> Keno(KenoResultQuery message)
        => View(message, KenoLotteryTableGameView);

    [AllowProtectedMode]
    public Task<IActionResult> Lottery(LotteryResultQuery message)
        => View(message, KenoLotteryTableGameView);

    [AllowProtectedMode]
    public Task<IActionResult> TableGames(TableGamesResultQuery message)
        => View(message, KenoLotteryTableGameView);

    [AllowProtectedMode]
    public Task<IActionResult> SportsLottery(SportsLotteryResultQuery message)
        => View(message, KenoLotteryTableGameView);

    [AllowProtectedMode]
    public Task<IActionResult> VirtualGames(VirtualGamesResultQuery message)
        => View(message, KenoLotteryTableGameView);
}