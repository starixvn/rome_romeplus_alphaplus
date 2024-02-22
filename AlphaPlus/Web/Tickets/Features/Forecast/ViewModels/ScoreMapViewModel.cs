using AlphaPlus.BetsForecast.Domain.Forecast;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Forecast.ViewModels;

public class ScoreMapViewModel
{
    public ScoreMapViewModel(IEnumerable<ScoreMapBetType>[] betTypes, IEnumerable<League> leagues)
    {
        BetTypes = betTypes;
        Leagues = leagues.Select(l => new SelectListItem
        {
            Text = l.LeagueName,
            Value = l.LeagueId.ToString()
        });
    }

    public ScoreMapViewModel(
        IEnumerable<ScoreMapBetType>[] betTypes,
        string liveBetTypes,
        string deadBetTypes,
        string matchIds,
        IEnumerable<ScoreMapResult> results)
    {
        BetTypes = betTypes;
        LiveBetTypes = liveBetTypes;
        DeadBetTypes = deadBetTypes;
        MatchIds = matchIds;
        Results = results;
        FromBetList = true;
    }

    public IEnumerable<ScoreMapBetType>[] BetTypes { get; }

    public string LiveBetTypes { get; }

    public string DeadBetTypes { get; }

    public string MatchIds { get; }

    public IEnumerable<SelectListItem> Leagues { get; }

    public IEnumerable<ScoreMapResult> Results { get; }

    public bool FromBetList { get; }
}