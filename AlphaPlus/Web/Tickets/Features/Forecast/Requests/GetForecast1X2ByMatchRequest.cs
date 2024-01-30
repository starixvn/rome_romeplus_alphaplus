using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class GetForecast1X2ByMatchRequest(IUserSession userSession) : RequestBase(userSession)
{
    public int MatchId { get; init; }

    public int BetType { get; init; }

    public bool Partial { get; init; }
}
