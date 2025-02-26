using AlphaPlus.BetsForecast.Domain.Forecast;

namespace AlphaPlus.Tickets.Site.Features.Forecast.ViewModels;

public class Forecast1X2ByMatchViewModel(int matchId, int betType, IEnumerable<Forecast1X2> data)
{
    public int MatchId { get; } = matchId;

    public int BetType { get; } = betType;

    public IEnumerable<Forecast1X2> Forecast1X2 { get; } = data;
}