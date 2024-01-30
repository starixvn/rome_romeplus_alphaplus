using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class GetSoccerResultRequest(IUserSession userSession) : RequestBase(userSession)
{
    public bool IsLive { get; init; } = true;

    public string MatchIds { get; init; } = "77819293,77819298";
}
