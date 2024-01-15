namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;

public record GetAHParam(
    byte CustLevel,
    int CustId,
    int MatchId,
    int BetType,
    int CurrentScore,
    string Language);
