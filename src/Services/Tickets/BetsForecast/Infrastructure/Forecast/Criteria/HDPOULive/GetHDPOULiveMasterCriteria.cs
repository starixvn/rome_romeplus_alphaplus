namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetHDPOULiveMasterCriteria(int masterId, int leagueId) : GetHDPOULiveBaseCriteria(leagueId)
{
    public int MRecommend { get; } = masterId;

    public override string GetSettingKey() => "Age_Forecast_SelByLeague_MAg";

    public override bool IsValid() => MRecommend > 0 && base.IsValid();
}