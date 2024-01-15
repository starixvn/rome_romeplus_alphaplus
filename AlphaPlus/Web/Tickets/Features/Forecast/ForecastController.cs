using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.Tickets.Site.Features.Forecast.Requests;
using AlphaPlus.Tickets.Site.Features.Forecast.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Forecast;

public class ForecastController : Controller//BaseController
{
    private const string PartialName = "Partial";
    private readonly IForecastService forecastService;
    private readonly IScoreMapService scoreMapService;

    public ForecastController(IForecastService forecastService, IScoreMapService scoreMapService)
        => (this.forecastService, this.scoreMapService) = (forecastService, scoreMapService);

    public async Task<IActionResult> Soccer()
    {
        var isLive = true;
        var leagues = await forecastService.GetSoccerLeagues(isLive, "en-US");
        var firstLeague = leagues.FirstOrDefault();
        var matches = firstLeague == null
            ? Enumerable.Empty<Match>()
            : await forecastService.GetSoccerMatches(firstLeague.LeagueId, isLive, "en-US");

        return View(Tuple.Create(
            leagues.Select(l => new SelectListItem(l.LeagueName, l.LeagueId.ToString())),
            matches.Select(m => new SelectListItem(m.Event, m.MatchId.ToString()))));
    }

    public async Task<IActionResult> HDPOULive(GetForecastRequest message)
    {
        // bettypes = 1: for HDP/OU/Live
        var leagues = await forecastService.GetRunningLeagues(message.ViewerLevel, message.ViewerId, "1", message.Language);
        var firstLeagueId = leagues.FirstOrDefault()?.LeagueId ?? 0;
        var selectedLeagueId = message.LeagueId > 0 ? message.LeagueId : firstLeagueId;
        var data = firstLeagueId == 0
            ? Enumerable.Empty<ForecastHDPOULive>()
            : await forecastService.GetHDPOULive(message.ViewerLevel, message.ViewerId, selectedLeagueId, message.Language);

        return View(
            $"{nameof(HDPOULive)}{(message.Partial ? PartialName : string.Empty)}",
            new ForecastViewModel<ForecastHDPOULive>(selectedLeagueId, leagues, data));
    }

    public async Task<IActionResult> HDPOULiveData(GetForecastRequest message)
        => View(
            nameof(HDPOULiveData),
            await forecastService.GetHDPOULive(message.ViewerLevel, message.ViewerId, message.LeagueId, message.Language));

    public async Task<IActionResult> AH(GetAHRequest message)
    {
        return View(
            $"{nameof(AH)}{(message.Partial ? PartialName : string.Empty)}",
            await forecastService.GetAH(message));
    }

    [ActionName("1X2")]
    public async Task<IActionResult> _1X2(GetForecastRequest message)
    {
        // bettypes = 5: for 1X2
        var leagues = await forecastService.GetRunningLeagues(message.ViewerLevel, message.ViewerId, "5", message.Language);
        var firstLeagueId = leagues.FirstOrDefault()?.LeagueId ?? 0;
        var selectedLeagueId = message.LeagueId > 0 ? message.LeagueId : firstLeagueId;
        var data = firstLeagueId == 0
            ? Enumerable.Empty<Forecast1X2>()
            : await forecastService.Get1X2(message.ViewerLevel, message.ViewerId, selectedLeagueId, message.Language);

        return View(
            $"1X2{(message.Partial ? PartialName : string.Empty)}",
            new ForecastViewModel<Forecast1X2>(selectedLeagueId, leagues, data));
    }

    [ActionName("1X2Data")]
    public async Task<IActionResult> _1X2Data(GetForecastRequest message)
        => View(await forecastService.Get1X2(message.ViewerLevel, message.ViewerId, message.LeagueId, message.Language));

    [ActionName("1X2ByMatch")]
    public async Task<IActionResult> _1X2ByMatch(GetForecast1X2ByMatchRequest message)
    {
        var data = await forecastService.Get1X2(
            message.ViewerLevel,
            message.ViewerId,
            message.MatchId,
            message.BetType,
            message.Language);

        if (message.Partial)
        {
            return View("1X2Data", data);
        }

        return View(new Forecast1X2ByMatchViewModel(message.MatchId, message.BetType, data));
    }

    public async Task<IActionResult> ScoreMap()
    {
        var betTypesTask = scoreMapService.GetBetTypes("en-US");
        var leaguesTask = scoreMapService.GetLeagues(23003140, "en-US");
        await Task.WhenAll(betTypesTask, leaguesTask);

        return View(new ScoreMapViewModel(await betTypesTask, await leaguesTask));
    }

    public async Task<IActionResult> GetScoreMapLeagues()
        => Json(await scoreMapService.GetLeagues(23003140, "en-US"));

    public async Task<IActionResult> GetScoreMapMatches(int leagueId)
        => Json(await scoreMapService.GetMatches(23003140, leagueId, "en-US"));

    public async Task<IActionResult> GetScoreMapResults(GetScoreMapResultRequest message)
        => View("ScoreMapResults", await scoreMapService.GetResults(message));

    public async Task<IActionResult> ScoreMapInformation()
        => View(await scoreMapService.GetBetTypes("en-US"));
}
