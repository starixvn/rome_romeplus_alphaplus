using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetHDPCriteria(byte custLevel, int custId, int matchId, int betType) : CriteriaBase
{
    public byte RoleId { get; } = custLevel;

    public int AgentId { get; } = custId;

    public int MatchId { get; } = matchId;

    public int BetType { get; } = betType;

    public override string GetSettingKey() => "Age_Forecast_SelHDP";

    public override bool IsValid()
        => AgentId > 0 && RoleId > 0 && MatchId > 0 && BetType > 0;
}