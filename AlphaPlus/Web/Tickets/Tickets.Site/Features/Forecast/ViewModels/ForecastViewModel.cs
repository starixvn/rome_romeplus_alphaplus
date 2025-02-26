using AlphaPlus.BetsForecast.Domain.Forecast;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Forecast.ViewModels;

public class ForecastViewModel<T>(int selectedLeague, IEnumerable<League> leagues, IEnumerable<T> data)
{
    public IEnumerable<SelectListItem> Leagues { get; }
        = leagues.Select(l => new SelectListItem
        {
            Text = l.LeagueName,
            Value = l.LeagueId.ToString(),
            Selected = selectedLeague == l.LeagueId
        });

    public IEnumerable<T> Forecast { get; } = data;
}