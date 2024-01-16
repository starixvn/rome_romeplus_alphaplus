using AlphaPlus.BetsForecast.Domain.Forecast;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Forecast.ViewModels;

public class ScoreMapViewModel(
    IEnumerable<ScoreMapBetType>[] betTypes,
    IEnumerable<League> leagues)
{
    public IEnumerable<ScoreMapBetType>[] BetTypes { get; } = betTypes;

    public IEnumerable<SelectListItem> Leagues { get; }
        = leagues.Select(l => new SelectListItem
        {
            Text = l.LeagueName,
            Value = l.LeagueId.ToString()
        });
}