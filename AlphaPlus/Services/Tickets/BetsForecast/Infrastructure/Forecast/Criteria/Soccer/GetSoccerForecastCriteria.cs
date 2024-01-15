using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetSoccerForecastCriteria(int custId, string matchIds, bool isLive) : CriteriaBase
{
    public int CustId { get; } = custId;

    public string MatchIdList { get; } = matchIds;

    public bool IsLive { get; } = isLive;

    public override string GetSettingKey() => "Age_NewTotalBetsForeCast_Main";

    public override bool IsValid() => CustId > 0 && !string.IsNullOrWhiteSpace(MatchIdList);
}