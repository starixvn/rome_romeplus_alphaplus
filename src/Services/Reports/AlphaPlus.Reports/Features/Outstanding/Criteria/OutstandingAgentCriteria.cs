namespace AlphaPlus.Reports.Features.Outstanding;

internal class OutstandingAgentCriteria(int custId, string productIds, byte levelType)
    : OutstandingMemberCriteria(custId, productIds)
{
    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => "Age_Rpt_Outstanding_Agent";
}