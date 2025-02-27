using AlphaPlus.Core.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class ThirdPartyController(IMediator mediator) : ResultBaseController(mediator)
{
    private const string IndexView = "~/Features/Results/ThirdParty/Views/Index.cshtml";

    [AllowProtectedMode]
    public Task<IActionResult> MaxGameResult(MaxGameResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> SportsLotteryDetailResult(SportsLotteryDetailResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> PPLiveCasinoResult(PPLiveCasinoResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> HabaneroResult(HabaneroResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> CGResult(CGResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> VGamingResult(VGamingResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> AdvantPlayResult(AdvantPlayResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> AdvantPlayMiniResult(AdvantPlayMiniResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> BBINResult(BBINResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> RngCasinoResult(RNGCasinoResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> MacauGamesResult(MacauGamesResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> PPResult(PPResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> SpadeGamingResult(SpadeGamingResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> RNGWarResult(RNGWarResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> IONCasinoResult(IONCasinoResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> PGSoftResult(PGSoftResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> JokerResult(JokerResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> LinkedGamesResult(LinkedGamesResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> BGResult(BGResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> CardClubResult(CardClubResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> FGGResult(FGGResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> VirtualGamesResult(VirtualGamesDetailResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> SEALResult(SEALResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> MGResult(MGResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> JiliResult(JiliResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> Live22Result(Live22ResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> WEResult(WEResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> UUSlotsResult(UUSlotsResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> SabaxDResult(SabaxDResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> FastSpinResult(FastSpinResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> FACHAIResult(FACHAIResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> NextspinResult(NextspinResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> ONCasinoResult(ONCasinoResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> PlaytechResult(PlaytechResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> SexyGamingResult(SexyGamingResultQuery message) => View(message, IndexView);

    [AllowProtectedMode]
    public Task<IActionResult> GPIResult(GPIResultQuery message) => View(message, IndexView);
        
    [AllowProtectedMode]
    public Task<IActionResult> GPILiveCasinoResult(GPILiveCasinoResultQuery message) => View(message, IndexView);

     [AllowProtectedMode]
    public Task<IActionResult> PlayStarResult(PlayStarResultQuery message) => View(message, IndexView);

     [AllowProtectedMode]
    public Task<IActionResult> ASKMESLOTResult(ASKMESLOTResultQuery message) => View(message, IndexView);

     [AllowProtectedMode]
    public Task<IActionResult> SabaKenoResult(SabaKenoResultQuery message) => View(message, IndexView);
}
