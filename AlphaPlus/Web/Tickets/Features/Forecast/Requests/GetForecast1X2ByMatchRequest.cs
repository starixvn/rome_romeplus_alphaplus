using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public record GetForecast1X2ByMatchRequest(int MatchId, int BetType, bool Partial) : RequestBase;
