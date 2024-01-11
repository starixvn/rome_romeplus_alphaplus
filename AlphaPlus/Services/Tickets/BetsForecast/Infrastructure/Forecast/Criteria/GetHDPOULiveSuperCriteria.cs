using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetHDPOULiveSuperCriteria(int superId, int leagueId, string language) : CriteriaBase
{
    public int SRecommend { get; } = superId;

    public int LeagueId { get; } = leagueId;

    public string Language { get; } = language;

    public int BaseOffsetHDP { get; }

    public int BaseOffsetOU { get; }

    public int BaseOffsetHTHDP { get; }

    public int BaseOffsetHTOU { get; }

    public override string GetSettingKey() => "Age_Forecast_SelByLeague_SAg";

    public override bool IsValid() => SRecommend > 0 && LeagueId > 0;
}
