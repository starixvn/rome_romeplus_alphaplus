namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;

public record GetScoreMapResultParam(
    byte CustLevel,
    int CustId,
    string MatchIds,
    string LiveBetTypes,
    string DeadBetTypes,
    string Language);
