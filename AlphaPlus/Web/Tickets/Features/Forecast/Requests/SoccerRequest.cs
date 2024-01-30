
using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class SoccerRequest(IUserSession userSession) : RequestBase(userSession)
{
    public bool IsLive { get; init; } = true;

    public int LeagueId { get; set; }
}