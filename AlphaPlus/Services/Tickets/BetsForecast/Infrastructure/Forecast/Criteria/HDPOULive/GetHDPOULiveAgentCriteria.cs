namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetHDPOULiveAgentCriteria(int agentId, int leagueId) : GetHDPOULiveBaseCriteria(leagueId)
{
    public int Recommend { get; } = agentId;

    public override string GetSettingKey() => "Age_Forecast_SelByLeague_Ag";

    public override bool IsValid() => Recommend > 0 && base.IsValid();
}