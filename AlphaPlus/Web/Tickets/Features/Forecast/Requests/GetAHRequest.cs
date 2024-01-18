using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public record GetAHRequest(int MatchId, int BetType, int Score, bool Partial) : RequestBase;
