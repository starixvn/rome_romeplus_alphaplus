namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetHDPOULiveSuperCriteria(int superId, int leagueId, string language)
    : GetHDPOULiveBaseCriteria(leagueId, language)
{
    public int SRecommend { get; } = superId;

    public override string GetSettingKey() => "Age_Forecast_SelByLeague_SAg";

    public override bool IsValid() => SRecommend > 0 && base.IsValid();
}
