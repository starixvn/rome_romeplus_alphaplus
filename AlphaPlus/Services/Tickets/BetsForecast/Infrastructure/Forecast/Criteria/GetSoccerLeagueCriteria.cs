using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetSoccerLeagueCriteria(bool isLive, bool isShowAll) : CriteriaBase
{
    public bool IsLive { get; } = isLive;

    public bool IsShowAll { get; } = isShowAll;

    public override string GetSettingKey() => "Age_NewTotalBetsForeCast_LeagueSel";

    public override bool IsValid() => true;
}