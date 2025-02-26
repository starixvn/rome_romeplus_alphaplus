using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal abstract class GetHDPOULiveBaseCriteria(int leagueId) : CriteriaBase
{
    public int LeagueId { get; } = leagueId;

    public string Language { get; } = string.Empty;

    public int BaseOffsetHDP { get; }

    public int BaseOffsetOU { get; }

    public int BaseOffsetHTHDP { get; }

    public int BaseOffsetHTOU { get; }

    public override bool IsValid() => LeagueId > 0;
}