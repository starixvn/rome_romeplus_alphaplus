using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class Get1X2ByLeagueCriteria(byte custLevel, int custId, int leagueId) : CriteriaBase
{
    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = Enumeration.FromValue<UserLevel>(custLevel);

    public int CustId { get; } = custId;

    public int LeagueId { get; } = leagueId;

    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_Forecast1x2_SelByLeague_SAg"
        : (CustLevel.IsMasterLevel()
            ? "Age_Forecast1x2_SelByLeague_MAg"
            : "Age_Forecast1x2_SelByLeague_Ag");

    public override bool IsValid() => CustId > 0 && LeagueId > 0;
}