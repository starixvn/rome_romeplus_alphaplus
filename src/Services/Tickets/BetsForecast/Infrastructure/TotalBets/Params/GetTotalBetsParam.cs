namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Params;

public record GetTotalBetsParam(byte CustLevel, int CustId, string Language, string SiteName);
