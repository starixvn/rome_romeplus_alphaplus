using AlphaPlus.BetsForecast.Domain.Shared;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class Get1X2ByLeagueCriteria(byte custLevel, int custId, int leagueId) : CriteriaBase
{
    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = (UserLevel)custLevel;

    public int CustId { get; } = custId;

    public int LeagueId { get; } = leagueId;

    public override string GetSettingKey()
        => CustLevel switch
        {
            UserLevel.Super => "Age_Forecast1x2_SelByLeague_SAg",
            UserLevel.Master => "Age_Forecast1x2_SelByLeague_MAg",
            UserLevel.Agent => "Age_Forecast1x2_SelByLeague_Ag",
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(CustLevel)),
        };

    public override bool IsValid() => CustId > 0 && LeagueId > 0;
}