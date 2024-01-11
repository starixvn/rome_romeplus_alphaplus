using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetHDPOULiveMasterCriteria(int masterId, int leagueId, string language) : CriteriaBase
{
    public int MRecommend { get; } = masterId;

    public int LeagueId { get; } = leagueId;

    public string Language { get; } = language;

    public int BaseOffsetHDP { get; }

    public int BaseOffsetOU { get; }

    public int BaseOffsetHTHDP { get; }

    public int BaseOffsetHTOU { get; }

    public override string GetSettingKey() => "Age_Forecast_SelByLeague_MAg";

    public override bool IsValid() => MRecommend > 0 && LeagueId > 0;
}