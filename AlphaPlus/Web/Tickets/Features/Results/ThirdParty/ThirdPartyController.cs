using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public partial class ResultsController(IMediator mediator) : ResultBaseController(mediator)
{
    protected const string IndexView = "~/Features/Results/ThirdParty/Views/Index.cshtml";

    public Task<IActionResult> MaxGameResult(MaxGameResultQuery message) => View(message, IndexView);

    public Task<IActionResult> SportsLotteryDetailResult(SportsLotteryDetailResultQuery message) => View(message, IndexView);

    public Task<IActionResult> PPLiveCasinoResult(PPLiveCasinoResultQuery message) => View(message, IndexView);

    public Task<IActionResult> HabaneroResult(HabaneroResultQuery message) => View(message, IndexView);

    public Task<IActionResult> CGResult(CGResultQuery message) => View(message, IndexView);

    public Task<IActionResult> VGamingResult(VGamingResultQuery message) => View(message, IndexView);

    public Task<IActionResult> AdvantPlayResult(AdvantPlayResultQuery message) => View(message, IndexView);

    public Task<IActionResult> AdvantPlayMiniResult(AdvantPlayMiniResultQuery message) => View(message, IndexView);

    public Task<IActionResult> BBINResult(BBINResultQuery message) => View(message, IndexView);

    public Task<IActionResult> RngCasinoResult(RNGCasinoResultQuery message) => View(message, IndexView);

    public Task<IActionResult> MacauGamesResult(MacauGamesResultQuery message) => View(message, IndexView);

    public Task<IActionResult> PPResult(PPResultQuery message) => View(message, IndexView);

    public Task<IActionResult> SpadeGamingResult(SpadeGamingResultQuery message) => View(message, IndexView);

    public Task<IActionResult> RNGWarResult(RNGWarResultQuery message) => View(message, IndexView);

    public Task<IActionResult> IONCasinoResult(IONCasinoResultQuery message) => View(message, IndexView);

    public Task<IActionResult> PGSoftResult(PGSoftResultQuery message) => View(message, IndexView);

    public Task<IActionResult> JokerResult(JokerResultQuery message) => View(message, IndexView);

    public Task<IActionResult> LinkedGamesResult(LinkedGamesResultQuery message) => View(message, IndexView);

    public Task<IActionResult> BGResult(BGResultQuery message) => View(message, IndexView);

    public Task<IActionResult> CardClubResult(CardClubResultQuery message) => View(message, IndexView);

    public Task<IActionResult> FGGResult(FGGResultQuery message) => View(message, IndexView);

    public Task<IActionResult> VirtualGamesResult(VirtualGamesDetailResultQuery message) => View(message, IndexView);

    public Task<IActionResult> SEALResult(SEALResultQuery message) => View(message, IndexView);

    public Task<IActionResult> MGResult(MGResultQuery message) => View(message, IndexView);

    public Task<IActionResult> JiliResult(JiliResultQuery message) => View(message, IndexView);

    public Task<IActionResult> Live22Result(Live22ResultQuery message) => View(message, IndexView);

    public Task<IActionResult> WEResult(WEResultQuery message) => View(message, IndexView);

    public Task<IActionResult> UUSlotsResult(UUSlotsResultQuery message) => View(message, IndexView);

    public ActionResult SabaxDResult(SabaxDResultQuery message) => View(IndexView, mediator.Send(message));
}