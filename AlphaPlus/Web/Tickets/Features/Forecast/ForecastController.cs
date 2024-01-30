using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using AlphaPlus.Tickets.Site.Features.Forecast.Requests;
using AlphaPlus.Tickets.Site.Features.Forecast.ViewModels;
using AlphaPlus.Tickets.Site.Features.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Forecast;

public class ForecastController : Controller//BaseController
{
    private const string PartialName = "Partial";
    private readonly IForecastService forecastService;
    private readonly IScoreMapService scoreMapService;
    private readonly ISoccerService soccerService;

    public ForecastController(
        IForecastService forecastService,
        IScoreMapService scoreMapService,
        ISoccerService soccerService)
        => (this.forecastService, this.scoreMapService, this.soccerService) = (forecastService, scoreMapService, soccerService);

    public async Task<IActionResult> Soccer(SoccerRequest message)
    {
        var leagues = await soccerService.GetLeagues(message.IsLive, message.Language);
        message.LeagueId = message.LeagueId == 0
            ? leagues.FirstOrDefault()?.LeagueId ?? 0
            : message.LeagueId;
        var matches = message.LeagueId == 0
            ? Enumerable.Empty<Match>()
            : await soccerService.GetMatches(message.LeagueId, message.IsLive, message.Language);

        var viewModel = new SoccerViewModel([
            new SelectListItem(Resources.Live, "true", message.IsLive),
            new SelectListItem(Resources.NonLive, "false", !message.IsLive)
        ], message.LeagueId, leagues, matches);

        return View(viewModel);
    }

    public async Task<IActionResult> GetSoccerLeagues(SoccerRequest message)
        => Json(await soccerService.GetLeagues(message.IsLive, message.Language));

    public async Task<IActionResult> GetSoccerMatches(SoccerRequest message)
        => Json(await soccerService.GetMatches(message.LeagueId, message.IsLive, message.Language));

    public async Task<IActionResult> GetSoccerResults(GetSoccerResultRequest message)
        => Json(await soccerService.GetResults(
            message.ViewerId,
            message.MatchIds,
            message.IsLive,
            message.Language));

    public async Task<IActionResult> HDPOULive(GetForecastRequest message)
    {
        // bettypes = 1: for HDP/OU/Live
        var leagues = await forecastService.GetRunningLeagues(
            message.ViewerLevel,
            message.ViewerId,
            BetTypeConstants.Handicap.ToString(),
            message.Language);
        message.LeagueId = message.LeagueId == 0
            ? leagues.FirstOrDefault()?.LeagueId ?? 0
            : message.LeagueId;
        var forecast = message.LeagueId == 0
            ? Enumerable.Empty<ForecastHDPOULive>()
            : await forecastService.GetHDPOULive(message);

        return View(
            $"{nameof(HDPOULive)}{(message.Partial ? PartialName : string.Empty)}",
            new ForecastViewModel<ForecastHDPOULive>(message.LeagueId, leagues, forecast));
    }

    public async Task<IActionResult> HDPOULiveData(GetForecastRequest message)
        => View(await forecastService.GetHDPOULive(message));

    public async Task<IActionResult> AH(GetAHRequest message)
        => View(
            $"{nameof(AH)}{(message.Partial ? PartialName : string.Empty)}",
            await forecastService.GetAH(message));

    [ActionName("1X2")]
    public async Task<IActionResult> _1X2(GetForecastRequest message)
    {
        // bettypes = 5: for 1X2
        var leagues = await forecastService.GetRunningLeagues(
            message.ViewerLevel,
            message.ViewerId,
            BetTypeConstants._1X2.ToString(),
            message.Language);
        message.LeagueId = message.LeagueId == 0
            ? leagues.FirstOrDefault()?.LeagueId ?? 0
            : message.LeagueId;
        var forecast = message.LeagueId == 0
            ? Enumerable.Empty<Forecast1X2>()
            : await forecastService.Get1X2(message);

        return View(
            $"1X2{(message.Partial ? PartialName : string.Empty)}",
            new ForecastViewModel<Forecast1X2>(message.LeagueId, leagues, forecast));
    }

    [ActionName("1X2Data")]
    public async Task<IActionResult> _1X2Data(GetForecastRequest message)
        => View(await forecastService.Get1X2(message));

    [ActionName("1X2ByMatch")]
    public async Task<IActionResult> _1X2ByMatch(GetForecast1X2ByMatchRequest message)
    {
        var forecast = await forecastService.Get1X2(message);

        if (message.Partial)
        {
            return View("1X2Data", forecast);
        }

        return View(new Forecast1X2ByMatchViewModel(message.MatchId, message.BetType, forecast));
    }

    public async Task<IActionResult> ScoreMap(RequestBase message)
    {
        var betTypesTask = scoreMapService.GetBetTypes(message.Language);
        var leaguesTask = scoreMapService.GetLeagues(message.ViewerId, message.Language);
        await Task.WhenAll(betTypesTask, leaguesTask);

        return View(new ScoreMapViewModel(await betTypesTask, await leaguesTask));
    }

    public async Task<IActionResult> GetScoreMapLeagues(RequestBase message)
        => Json(await scoreMapService.GetLeagues(message.ViewerId, message.Language));

    public async Task<IActionResult> GetScoreMapMatches(GetForecastRequest message)
        => Json(await scoreMapService.GetMatches(message.ViewerId, message.LeagueId, message.Language));

    public async Task<IActionResult> ScoreMapResults(GetScoreMapResultRequest message)
    {
        ViewData["IsRefresh"] = message.IsRefresh;

        return View(await scoreMapService.GetResults(message));
    }

    public async Task<IActionResult> ScoreMapInformation(RequestBase message)
        => View(await scoreMapService.GetBetTypes(message.Language));
}
