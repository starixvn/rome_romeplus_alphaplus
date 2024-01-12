using AlphaPlus.BetsForecast.Domain.Forecast;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Forecast.ViewModels;

public class ScoreMapViewModel(
    IEnumerable<ScoreMapBetType>[] scoreMapBetTypes,
    IEnumerable<League> leagues)
{
    public IEnumerable<ScoreMapBetType>[] ScoreMapBetTypes { get; } = scoreMapBetTypes;

    public IEnumerable<SelectListItem> Leagues { get; }
        = leagues.Select(l => new SelectListItem
        {
            Text = l.LeagueName,
            Value = l.LeagueId.ToString()
        });
}