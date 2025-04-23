namespace AlphaPlus.Reports.Features.CommissionByBetType;

internal class CommissionByBetTypeAgentCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDay,
    byte levelType) : CommissionByBetTypeMemberCriteria(custId, fromDate, toDate, moveBetsDay)
{
    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => "Age_Rpt_CommByBettype_Agent";
}