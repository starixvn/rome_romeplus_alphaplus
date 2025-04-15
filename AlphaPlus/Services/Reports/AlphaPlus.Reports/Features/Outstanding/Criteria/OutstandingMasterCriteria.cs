namespace AlphaPlus.Reports.Features.Outstanding;

internal class OutstandingMasterCriteria(int custId, string productIds, byte levelType)
    : OutstandingAgentCriteria(custId, productIds, levelType)
{
    public override string GetSettingKey() => "Age_Rpt_Outstanding_Master";
}