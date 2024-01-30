using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class GetScoreMapResultRequest(IUserSession userSession) : RequestBase(userSession)
{
    public string MatchIds { get; init; }

    public string LiveBetTypes { get; init; }

    public string DeadBetTypes { get; init; }

    public bool IsRefresh { get; init; }
}