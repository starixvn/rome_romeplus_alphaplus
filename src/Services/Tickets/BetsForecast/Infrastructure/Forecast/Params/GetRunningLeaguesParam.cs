namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;

public record GetRunningLeaguesParam(
    byte CustLevel,
    int CustId,
    string BetTypes,
    string Language,
    string SiteName);
