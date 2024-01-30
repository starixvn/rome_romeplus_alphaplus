using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class GetForecastRequest(IUserSession userSession) : RequestBase(userSession)
{
    public int LeagueId { get; set; }

    public bool Partial { get; init; }
}
