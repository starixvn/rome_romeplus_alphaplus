using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetRunningLeagueMasterCriteria(int masterId, string betTypes) : CriteriaBase
{
    public int MAgId { get; } = masterId;

    public string BetTypes { get; } = betTypes;

    public override string GetSettingKey()
        => "Age_LeagueSelRunning_MAg";

    public override bool IsValid()
        => MAgId > 0 && !string.IsNullOrWhiteSpace(BetTypes);
}
