using AlphaPlus.BetsForecast.Domain.Forecast;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Forecast.ViewModels;

public class SoccerViewModel(
    IEnumerable<SelectListItem> liveStatus,
    IEnumerable<League> leagues,
    IEnumerable<Match> matches)
{
    public IEnumerable<SelectListItem> LiveStatus { get; } = liveStatus;

    public IEnumerable<SelectListItem> Leagues { get; }
        = leagues.Select(l => new SelectListItem(l.LeagueName, l.LeagueId.ToString()));

    public IEnumerable<SelectListItem> Matches { get; }
        = matches.Select(m => new SelectListItem(m.Event, m.MatchId.ToString()));
}
