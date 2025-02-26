namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetOUCriteria(byte custLevel, int custId, int matchId, int betType)
    : GetHDPCriteria(custLevel, custId, matchId, betType)
{
    public override string GetSettingKey() => "Age_Forecast_SelOU";
}
