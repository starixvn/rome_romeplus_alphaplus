namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;

public record GetForecastParam(byte CustLevel, int CustId, int LeagueId, string Language);
