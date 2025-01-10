namespace AlphaPlus.Reports.Features.CommissionByBetType;

internal class CommissionByBetTypeMasterCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDay,
    byte levelType) : CommissionByBetTypeAgentCriteria(custId, fromDate, toDate, moveBetsDay, levelType)
{
    public override string GetSettingKey() => "Age_Rpt_CommByBettype_Master";
}