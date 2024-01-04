using AlphaPlus.BetsForecast.Domain.Forecast;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Forecast;

public class ForecastController : Controller//BaseController
{
    private readonly IForecastService forecastService;

    public ForecastController(IForecastService forecastService)
        => this.forecastService = forecastService;

    public async Task<IActionResult> Soccer()
    {
        var isLive = true;
        var leagues = await forecastService.GetSoccerLeagues(isLive);
        var firstLeague = leagues.FirstOrDefault();
        var matches = firstLeague == null
            ? Enumerable.Empty<Match>()
            : await forecastService.GetSoccerMatches(firstLeague.LeagueId, isLive);

        return View(Tuple.Create(
            leagues.Select(l => new SelectListItem(l.LeagueName, l.LeagueId.ToString())),
            matches.Select(m => new SelectListItem(m.Event, m.MatchId.ToString()))));
    }
}
