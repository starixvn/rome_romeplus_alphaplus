using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetRunningLeagueSuperCriteria(int superId, string betTypes) : CriteriaBase
{
    public int SAgId { get; } = superId;

    public string BetTypes { get; } = betTypes;

    public override string GetSettingKey()
        => "Age_LeagueSelRunning_SAg";

    public override bool IsValid()
        => SAgId > 0 && !string.IsNullOrWhiteSpace(BetTypes);
}
