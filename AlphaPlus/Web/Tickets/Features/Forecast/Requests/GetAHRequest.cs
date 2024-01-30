using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class GetAHRequest(IUserSession userSession) : RequestBase(userSession)
{
    public int MatchId { get; init; }

    public int BetType { get; init; }

    public int Score { get; init; }

    public bool Partial { get; init; }
}
