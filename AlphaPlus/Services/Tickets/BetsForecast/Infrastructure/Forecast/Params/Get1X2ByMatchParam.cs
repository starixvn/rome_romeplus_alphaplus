namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;

public record Get1X2ByMatchParam(
    byte CustLevel,
    int CustId,
    int MatchId,
    int BetType,
    string Language);
