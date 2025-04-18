namespace AlphaPlus.Identity.API.Features.Authorization;

public class CheckBelongingConsolidatedGroupCommand(int sessionCustId, int subAccId, long transId, int? custId = null)
    : CheckBelongingCommand(sessionCustId, subAccId, transId, custId)
{
    public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_DBCUST";
}
