namespace AlphaPlus.Core.Criteria;

public class CheckConsolidateGroupBelongingCommand(int sessionCustId, int subAccId, long transId, int? custId = null)
    : CheckBelongingCommand(sessionCustId, subAccId, transId, custId)
{
    public override string GetSettingKey() => "Age_Rpt_CustomerConsolidatedGroup_CheckBelonging";
}
