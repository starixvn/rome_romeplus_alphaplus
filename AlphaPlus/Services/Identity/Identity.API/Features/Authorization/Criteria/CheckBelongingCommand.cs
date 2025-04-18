namespace AlphaPlus.Identity.API.Features.Authorization;

public class CheckBelongingCommand(
    int sessionCustId,
    int subAccId,
    long transId,
    int? custId = null) : CheckBelongingBaseCommand(sessionCustId, subAccId)
{
    public long TransId { get; } = transId;

    public int? CustId { get; } = custId;

    public override string GetSettingKey() => "Age_CustomerSelCheckBelonging";
}
