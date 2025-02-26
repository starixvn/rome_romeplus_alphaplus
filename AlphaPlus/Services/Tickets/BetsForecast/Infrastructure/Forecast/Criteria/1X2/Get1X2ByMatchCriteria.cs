using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class Get1X2ByMatchCriteria(byte custLevel, int custId, int matchId, int betType) : CriteriaBase
{
    public byte RoleId { get; } = custLevel;

    public int CustId { get; } = custId;

    public int MatchId { get; } = matchId;

    public int BetType { get; } = betType;

    public override string GetSettingKey() => "Age_Forecast_Sel1X2";

    public override bool IsValid()
        => RoleId > 1 && CustId > 0 && MatchId > 0 && BetType > 0;
}