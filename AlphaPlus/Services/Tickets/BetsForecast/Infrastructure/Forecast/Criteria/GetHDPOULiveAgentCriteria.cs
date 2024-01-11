using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetHDPOULiveAgentCriteria(int agentId, int leagueId, string language) : CriteriaBase
{
    public int Recommend { get; } = agentId;

    public int LeagueId { get; } = leagueId;

    public string Language { get; } = language;

    public int BaseOffsetHDP { get; }

    public int BaseOffsetOU { get; }

    public int BaseOffsetHTHDP { get; }

    public int BaseOffsetHTOU { get; }

    public override string GetSettingKey() => "Age_Forecast_SelByLeague_Ag";

    public override bool IsValid() => Recommend > 0 && LeagueId > 0;
}